using System;
using UnityEngine;

namespace CustomMath
{
    //3
    public class CustomQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        // elemento neutro
        private static readonly CustomQuaternion _identity = new(0f, 0f, 0f, 1f);

        public static CustomQuaternion identity => _identity;

        public const float kEpsilon = 1E-06f;

        public CustomQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public CustomQuaternion normalized => Normalize(this);


        /// <summary>
        /// Inverts a rotation.
        /// https://www.mathworks.com/help/aeroblks/quaternioninverse.html
        /// </summary>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static CustomQuaternion Inverse(CustomQuaternion rotation)
        {
            var sqrMag = Dot(rotation, rotation);

            if (sqrMag < kEpsilon)
                return identity;

            return new(
                -rotation.x / sqrMag,
                -rotation.y / sqrMag,
                -rotation.z / sqrMag,
                rotation.w / sqrMag
            );
        }

        /// <summary>
        /// Inverts this rotation.
        /// </summary>
        /// <returns></returns>
        public CustomQuaternion Inverse()
        {
            return Inverse(this);
        }

        /// <summary>
        /// Receives euler angles in radians and returns the corresponding quaternion.
        /// https://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
        /// </summary>
        /// <param name="euler"></param>
        /// <returns></returns>
        private static CustomQuaternion FromEulerRad(Vec3 euler)
        {
            var roll = euler.x;
            var pitch = euler.y;
            var yaw = euler.z;

            float cr = Mathf.Cos(roll * 0.5f);
            float sr = Mathf.Sin(roll * 0.5f);
            float cp = Mathf.Cos(pitch * 0.5f);
            float sp = Mathf.Sin(pitch * 0.5f);
            float cy = Mathf.Cos(yaw * 0.5f);
            float sy = Mathf.Sin(yaw * 0.5f);

            float w = cr * cp * cy + sr * sp * sy;
            float x = sr * cp * cy - cr * sp * sy;
            float y = cr * sp * cy + sr * cp * sy;
            float z = cr * cp * sy - sr * sp * cy;

            return Normalize(new(x, y, z, w));
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

        public static implicit operator Quaternion(CustomQuaternion a)
        {
            return new Quaternion(a.x, a.y, a.z, a.w);
        }

        /// <summary>
        /// If it returns 1 the quaternions are equal, 
        /// if it returns -1 they are opposite.
        /// If it returns 0 they are orthogonal.
        /// https://www.cs.ucdavis.edu/~amenta/3dphoto/quaternion.pdf
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Dot(CustomQuaternion a, CustomQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static CustomQuaternion Euler(Vec3 euler)
        {
            return FromEulerRad(euler * Mathf.Deg2Rad);
        }

        public static CustomQuaternion Normalize(CustomQuaternion q)
        {
            float mag = Mathf.Sqrt(Dot(q, q));
            if (mag < Mathf.Epsilon)
            {
                return identity;
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
