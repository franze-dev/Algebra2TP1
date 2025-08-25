using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UIElements;

public class CustomQuaternion : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public float w;

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

    public Vector3 EulerAngles
    {
        get => MakePositive(ToEulerRad(this) * 57.29578f);
        set
        {
            CustomQuaternion q = FromEulerRad(value * (MathF.PI / 180f));

            Set(q.x, q.y, q.z, q.w);
        }
    }

    public CustomQuaternion normalized => Normalize(this);

    public static CustomQuaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        var from = fromDirection.normalized;
        var to = toDirection.normalized;

        Vector3 axis;
        var dot = Vector3.Dot(from, to);

        if (IsEqualUsingDot(dot))
            return Identity;

        // if vectors are opposite
        if (dot < -1 + kEpsilon)
        {
            // find an axis that is orthogonal to 'from'
            axis = Vector3.Cross(Vector3.right, from);
            if (axis.sqrMagnitude < kEpsilon)
                axis = Vector3.Cross(from, Vector3.up);

            axis.Normalize();
            return AngleAxis(180f, axis);
        }

        axis = Vector3.Cross(from, to);
        var angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f));

        return AngleAxis(angle, axis);
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

    private static CustomQuaternion FromEulerRad(Vector3 euler)
    {
        var sinAngle = Mathf.Sin(Mathf.Deg2Rad * euler.z * 0.5f);
        var cosAngle = Mathf.Cos(Mathf.Deg2Rad * euler.z * 0.5f);
        var qz = new CustomQuaternion(0, 0, sinAngle, cosAngle);

        sinAngle = Mathf.Sin(Mathf.Deg2Rad * euler.x * 0.5f);
        cosAngle = Mathf.Cos(Mathf.Deg2Rad * euler.x * 0.5f);
        var qx = new CustomQuaternion(sinAngle, 0, 0, cosAngle);

        sinAngle = Mathf.Sin(Mathf.Deg2Rad * euler.y * 0.5f);
        cosAngle = Mathf.Cos(Mathf.Deg2Rad * euler.y * 0.5f);
        var qy = new CustomQuaternion(0, sinAngle, 0, cosAngle);

        return qy * qx * qz;
    }

    private static Vector3 ToEulerRad(CustomQuaternion rotation)
    {
        var pitch = Mathf.Atan2(
            2f * (rotation.w * rotation.x + rotation.y * rotation.z),
            rotation.w * rotation.w - 
            rotation.x * rotation.x - 
            rotation.y * rotation.y + 
            rotation.z * rotation.z
        );

        var yaw = Mathf.Asin(-2f * (rotation.x * rotation.z - rotation.w * rotation.y));

        var roll = Mathf.Atan2(
            2f * (rotation.w * rotation.z + rotation.x * rotation.y),
            rotation.w * rotation.w + 
            rotation.x * rotation.x - 
            rotation.y * rotation.y - 
            rotation.z * rotation.z
        );

        return new Vector3(pitch, yaw, roll);
    }

    private static void ToAxisAngleRad(CustomQuaternion q, out Vector3 axis, out float angle)
    {

    }

    public static CustomQuaternion AngleAxis(float angle, Vector3 axis)
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

    public static CustomQuaternion LookRotation(Vector3 forward)
    {
        return LookRotation(forward, Vector3.up);
    }

    public static CustomQuaternion LookRotation(Vector3 forward, Vector3 upwards)
    {

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

    public static Vector3 operator *(CustomQuaternion rotation, Vector3 point)
    {

    }

    private static bool IsEqualUsingDot(float dot)
    {
        return dot > 0.999999f;
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

    public void SetLookRotation(Vector3 view)
    {
        Vector3 up = Vector3.up;
        SetLookRotation(view, up);
    }

    public void SetLookRotation(Vector3 view, Vector3 up)
    {
        CustomQuaternion q = LookRotation(view, up);
        Set(q.x, q.y, q.z, q.w);
    }

    public static float Angle(CustomQuaternion a, CustomQuaternion b)
    {
        float posAngle = Mathf.Abs(Dot(a, b));

        return IsEqualUsingDot(posAngle) ?
            0f :
            (Mathf.Acos(posAngle) * 2f * Mathf.Rad2Deg); ;
    }

    private static Vector3 MakePositive(Vector3 euler)
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

    public static CustomQuaternion Euler(Vector3 euler)
    {
        return FromEulerRad(euler * (MathF.PI / 180f));
    }

    public void ToAngleAxis(out float angle, out Vector3 axis)
    {
        ToAxisAngleRad(this, out axis, out angle);
        angle *= 57.29578f;
    }

    public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
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
