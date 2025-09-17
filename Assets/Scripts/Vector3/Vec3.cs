using UnityEngine;
using System;
namespace CustomMath
{
    [Serializable]
    public class Vec3
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Vector magnitude squared. Useful for comparisons. It avoids an expensive square root calculation.
        /// </summary>
        public float sqrMagnitude { get => x * x + y * y + z * z; }
        /// <summary>
        /// This vector with a magnitude of 1.
        /// </summary>
        public Vec3 normalized { get => this / magnitude; }
        /// <summary>
        /// The length/norm of the vector.
        /// </summary>
        public float magnitude { get => Magnitude(this); }

        #endregion

        #region constants
        /// <summary>
        /// A small value that is used for floating point comparisons.
        /// </summary>
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 one { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 back { get { return -forward; } }
        public static Vec3 left { get { return -right; } }
        public static Vec3 down { get { return -up; } }
        public static Vec3 positiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 negativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #region Operators
        /// <summary>
        /// Calculculates if two vectors are equal using the squared magnitude of their difference.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
        }

        /// <summary>
        /// Vector multiplied by a scalar. It can scale the vector up or down. (If the scalar is negative, the vector direction is reversed.)
        /// </summary>
        /// <param name="v3"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
        }

        /// <summary>
        /// While not mathematically correct, it's useful for multiplying data saved in vectors.
        /// It'd be correct as long as they're not treated as actual vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.x * b.x,
                a.y * b.y,
                a.z * b.z
                );
        }

        /// <summary>
        /// Vector scaled down by a scalar. If the scalar is less than or equal to zero, the original vector is returned.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vec3 operator /(Vec3 a, float scalar)
        {
            if (scalar <= 0)
                return a;

            return new Vec3(a.x / scalar, a.y / scalar, a.z / scalar);
        }

        public static implicit operator Vector3(Vec3 a)
        {
            return new Vector3(a.x, a.y, a.z);
        }

        public static implicit operator Vector2(Vec3 a)
        {
            return new Vector2(a.x, a.y);
        }
        #endregion

        #region Functions

        /// <summary>
        /// Magnitude of a vector. It is always a positive/zero value.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float Magnitude(Vec3 vector)
        {
            return Magnitude(vector.x, vector.y, vector.z);
        }

        public static float Magnitude(float x, float y, float z)
        {
            return Mathf.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Returns a vector that is perpendicular to the two input vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );
        }

        #endregion
    }
}