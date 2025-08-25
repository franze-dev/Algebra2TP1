using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Internal;

namespace CustomMath
{

    public class CustomQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        Transform t;

        private static readonly CustomQuaternion _identity = new(0f, 0f, 0f, 1f);

        public static CustomQuaternion Identity => _identity;

        public const float kEpsilon = 1E-06f;

        public CustomQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vec3 EulerAngles
        {
            get => MakePositive(ToEulerRad(this) * 57.29578f);
            set
            {
                CustomQuaternion q = FromEulerRad(value * (MathF.PI / 180f));

                Set(q.x, q.y, q.z, q.w);
            }
        }

        public CustomQuaternion normalized => Normalize(this);

        public static CustomQuaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            var from = fromDirection.normalized;
            var to = toDirection.normalized;

            Vec3 axis;
            var dot = Vec3.Dot(from, to);

            if (IsEqualUsingDot(dot))
                return Identity;

            // if vectors are opposite
            if (dot < -1 + kEpsilon)
            {
                // find an axis that is orthogonal to 'from'
                axis = Vec3.Cross(Vec3.right, from);
                if (axis.sqrMagnitude < kEpsilon)
                    axis = Vec3.Cross(from, Vec3.up);

                axis.Normalize();
                return AngleAxis(180f, axis);
            }

            axis = Vec3.Cross(from, to);
            var angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f));

            return AngleAxis(angle * Mathf.Rad2Deg, axis);
        }

        public static CustomQuaternion Inverse(CustomQuaternion rotation)
        {
            var sqrMag = Dot(rotation, rotation);

            if (sqrMag < kEpsilon)
                return Identity;

            return new(
                -rotation.x / sqrMag,
                -rotation.y / sqrMag,
                -rotation.z / sqrMag,
                rotation.w / sqrMag
            );
        }

        public static CustomQuaternion Slerp(CustomQuaternion a, CustomQuaternion b, float t)
        {
            t = Mathf.Clamp01(t);

            return SlerpUnclamped(a, b, t);
        }

        public static CustomQuaternion SlerpUnclamped(CustomQuaternion a, CustomQuaternion b, float t)
        {
            var nA = a.normalized;
            var nB = b.normalized;

            if (Dot(nA, nB) < 0f)
                // shortest interpolation path
                nB = new(-nB.x, -nB.y, -nB.z, -nB.w);

            var angle = Angle(a, b) * Mathf.Deg2Rad;
            var sin = Mathf.Sin(angle);

            if (sin < kEpsilon)
                // if angle is too small, use linear interpolation to avoid division by zero
                return LerpUnclamped(a, b, t);

            var resA = Mathf.Sin((1 - t) * angle) / sin;
            var resB = Mathf.Sin(t * angle) / sin;

            return new(
                nA.x * resA + nB.x * resB,
                nA.y * resA + nB.y * resB,
                nA.z * resA + nB.z * resB,
                nA.w * resA + nB.w * resB
            );
        }

        public static CustomQuaternion Lerp(CustomQuaternion a, CustomQuaternion b, float t)
        {
            t = Mathf.Clamp01(t);
            return LerpUnclamped(a, b, t);
        }

        public static CustomQuaternion LerpUnclamped(CustomQuaternion a, CustomQuaternion b, float t)
        {
            var lerpT = (1 - t);

            var q = new CustomQuaternion(
                lerpT * a.x + t * b.x,
                lerpT * a.y + t * b.y,
                lerpT * a.z + t * b.z,
                lerpT * a.w + t * b.w
            );

            return Normalize(q);
        }

        /// <summary>
        /// Receives euler angles in radians and returns the corresponding quaternion.
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        private static CustomQuaternion FromEulerRad(Vec3 euler)
        {
            var sinAngle = Mathf.Sin(euler.z * 0.5f);
            var cosAngle = Mathf.Cos(euler.z * 0.5f);
            var qz = new CustomQuaternion(0, 0, sinAngle, cosAngle);

            sinAngle = Mathf.Sin(euler.x * 0.5f);
            cosAngle = Mathf.Cos(euler.x * 0.5f);
            var qx = new CustomQuaternion(sinAngle, 0, 0, cosAngle);

            sinAngle = Mathf.Sin(euler.y * 0.5f);
            cosAngle = Mathf.Cos(euler.y * 0.5f);
            var qy = new CustomQuaternion(0, sinAngle, 0, cosAngle);

            return qz * qx * qy;
        }

        private static Vec3 ToEulerRad(CustomQuaternion rotation)
        {
            float sRoll = 2f * (rotation.w * rotation.x + rotation.y * rotation.z);
            float cRoll = 1f - 2f * (rotation.x * rotation.x + rotation.y * rotation.y);
            float roll = Mathf.Atan2(sRoll, cRoll);

            float sPitch = 2f * (rotation.w * rotation.y - rotation.z * rotation.x);
            var sign = Mathf.Sign(sPitch);
            float pitch = Mathf.Abs(sPitch) >= 1f ?
                Mathf.Abs(Mathf.PI / 2f) * sign :
                Mathf.Asin(sPitch);

            float sYaw = 2f * (rotation.w * rotation.z + rotation.x * rotation.y);
            float cYaw = 1f - 2f * (rotation.y * rotation.y + rotation.z * rotation.z);
            float yaw = Mathf.Atan2(sYaw, cYaw);

            return new Vec3(pitch, yaw, roll);
        }

        private static void ToAxisAngleRad(CustomQuaternion q, out Vec3 axis, out float angle)
        {
            q.Normalize();

            // If I used q == _identity here, _identity could be negative. While mathematically correct,
            // it would fail the dot comparison in IsEqualUsingDot (== operator)
            if (Mathf.Abs(q.x) < kEpsilon &&
                Mathf.Abs(q.y) < kEpsilon &&
                Mathf.Abs(q.z) < kEpsilon)
            {
                axis = new Vec3(1f, 0f, 0f);
                angle = 0f;
                return;
            }

            angle = 2f * Mathf.Acos(q.w);

            var axisMag = Mathf.Sqrt(1f - q.w * q.w);

            if (axisMag < kEpsilon)
                axis = new Vec3(1f, 0f, 0f);
            else
                axis = new Vec3(q.x / axisMag, q.y / axisMag, q.z / axisMag);
        }

        public static CustomQuaternion AngleAxis(float angle, Vec3 axis)
        {
            if (axis.sqrMagnitude < kEpsilon)
                return Identity;

            axis = axis.normalized;

            float half = angle * Mathf.Deg2Rad * 0.5f;

            float sin = Mathf.Sin(half);
            float cos = Mathf.Cos(half);

            return new CustomQuaternion(
                axis.x * sin,
                axis.y * sin,
                axis.z * sin,
                cos
                );
        }

        public static CustomQuaternion LookRotation(Vec3 forward)
        {
            return LookRotation(forward, Vec3.up);
        }

        /// <summary>
        /// This constructs a quaternion representing a rotation that looks in the 'forward' direction.
        /// </summary>
        /// <param name="forward">This vector must be orthogonal to the 'upwards' vector</param>
        /// <param name="upwards">This vector must be orthogonal to the 'forward' vector</param>
        /// <returns></returns>
        public static CustomQuaternion LookRotation(Vec3 forward, Vec3 upwards)
        {
            var fw = forward.normalized;
            var up = upwards.normalized;
            var right = Vec3.Cross(up, fw).normalized;

            CustomMatrix4x4 m = new();

            m.SetColumn(0, new Vector4(right.x, right.y, right.z, 0));
            m.SetColumn(1, new Vector4(up.x, up.y, up.z, 0));
            m.SetColumn(2, new Vector4(fw.x, fw.y, fw.z, 0));
            m.SetColumn(3, new Vector4(0, 0, 0, 1));

            CustomQuaternion q = m.rotation;

            return q;
        }

        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public static CustomQuaternion operator *(CustomQuaternion q1, CustomQuaternion q2)
        {
            return new CustomQuaternion(
                q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
                q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z,
                q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x,
                q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z);
        }

        // Rotates a point by a quaternion rotation
        public static Vec3 operator *(CustomQuaternion rotation, Vec3 point)
        {
            Vec3 u = new(rotation.x, rotation.y, rotation.z);
            float w = rotation.w;

            Vec3 uv = Vec3.Cross(u, point);
            Vec3 uuv = Vec3.Cross(u, uv);

            return point + (uv * (2f * w)) + (uuv * 2f);
        }

        private static bool IsEqualUsingDot(float dot)
        {
            return dot > 0.999999f || dot < -0.999999f;
        }

        public static bool operator ==(CustomQuaternion lhs, CustomQuaternion rhs)
        {
            return IsEqualUsingDot(Dot(lhs, rhs));
        }

        public static bool operator !=(CustomQuaternion lhs, CustomQuaternion rhs)
        {
            return !(lhs == rhs);
        }

        public static float Dot(CustomQuaternion a, CustomQuaternion b)
        {
            return
                a.x * b.x +
                a.y * b.y +
                a.z * b.z +
                a.w * b.w;
        }

        public void SetLookRotation(Vec3 view)
        {
            Vec3 up = Vec3.up;
            SetLookRotation(view, up);
        }

        public void SetLookRotation(Vec3 view, Vec3 up)
        {
            CustomQuaternion q = LookRotation(view, up);
            Set(q.x, q.y, q.z, q.w);
        }

        public static float Angle(CustomQuaternion a, CustomQuaternion b)
        {
            float dot = Mathf.Clamp01(Mathf.Abs(Dot(a, b)));

            return IsEqualUsingDot(dot) ?
                0f :
                (Mathf.Acos(dot) * 2f * Mathf.Rad2Deg); ;
        }

        private static Vec3 MakePositive(Vec3 euler)
        {
            float min = kEpsilon;
            float max = 360f + kEpsilon;
            if (euler.x < min)
                euler.x += 360f;
            else if (euler.x > max)
                euler.x -= 360f;

            if (euler.y < min)
                euler.y += 360f;
            else if (euler.y > max)
                euler.y -= 360f;

            if (euler.z < min)
                euler.z += 360f;
            else if (euler.z > max)
                euler.z -= 360f;

            return euler;
        }

        public static CustomQuaternion Euler(Vec3 euler)
        {
            return FromEulerRad(euler * (MathF.PI / 180f));
        }

        public void ToAngleAxis(out float angle, out Vec3 axis)
        {
            ToAxisAngleRad(this, out axis, out angle);
            angle *= 57.29578f;
        }

        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            CustomQuaternion q = FromToRotation(fromDirection, toDirection);
            Set(q.x, q.y, q.z, q.w);
        }

        public static CustomQuaternion RotateTowards(CustomQuaternion from, CustomQuaternion to, float maxDegreesDelta)
        {
            float angle = Angle(from, to);
            if (angle == 0f)
            {
                return to;
            }

            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
        }

        public static CustomQuaternion Normalize(CustomQuaternion q)
        {
            float mag = Mathf.Sqrt(Dot(q, q));
            if (mag < Mathf.Epsilon)
            {
                return Identity;
            }

            return new CustomQuaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
        }

        public void Normalize()
        {
            CustomQuaternion q = Normalize(this);
            Set(q.x, q.y, q.z, q.w);
        }
    }
}
