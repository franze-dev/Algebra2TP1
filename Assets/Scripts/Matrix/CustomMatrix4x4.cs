using System;
using UnityEngine;

namespace CustomMath
{
    /// <summary>
    /// Created to make it easier for me to make matrices 4x4
    /// </summary>
    public struct Vec4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vec4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static bool operator ==(Vec4 lhs, Vec4 rhs)
        {
            return Mathf.Abs(lhs.x - rhs.x) < Mathf.Epsilon &&
                   Mathf.Abs(lhs.y - rhs.y) < Mathf.Epsilon &&
                   Mathf.Abs(lhs.z - rhs.z) < Mathf.Epsilon &&
                   Mathf.Abs(lhs.w - rhs.w) < Mathf.Epsilon;
        }

        public static bool operator !=(Vec4 lhs, Vec4 rhs)
        {
            return !(lhs == rhs);
        }
    }

    //4
    public class CustomMatrix4x4
    {

        // The components of the matrix
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;

        // The multiplicative identity of matrices (M * I = I * M = M)
        // elemento neutro
        public static CustomMatrix4x4 identity => new CustomMatrix4x4(new Vec4(1f, 0f, 0f, 0f),
                                                                      new Vec4(0f, 1f, 0f, 0f),
                                                                      new Vec4(0f, 0f, 1f, 0f),
                                                                      new Vec4(0f, 0f, 0f, 1f));
        public CustomMatrix4x4(Vec4 column0, Vec4 column1, Vec4 column2, Vec4 column3)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m03 = column3.x;

            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
            m13 = column3.y;

            m20 = column0.z;
            m21 = column1.z;
            m22 = column2.z;
            m23 = column3.z;

            m30 = column0.w;
            m31 = column1.w;
            m32 = column2.w;
            m33 = column3.w;
        }

        public CustomMatrix4x4()
        {
            Set(CustomMatrix4x4.identity);
        }

        /// <summary>
        /// It multiplies each row and column by each row and column of the other. 
        /// For example: m00 a.row0 dot b.col0
        ///              m12 a.row1 dot b.col2
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CustomMatrix4x4 operator *(CustomMatrix4x4 a, CustomMatrix4x4 b)
        {
            CustomMatrix4x4 result = new();
            result.m00 = a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30;
            result.m01 = a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31;
            result.m02 = a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32;
            result.m03 = a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33;

            result.m10 = a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30;
            result.m11 = a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31;
            result.m12 = a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32;
            result.m13 = a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33;

            result.m20 = a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30;
            result.m21 = a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31;
            result.m22 = a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32;
            result.m23 = a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33;

            result.m30 = a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30;
            result.m31 = a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31;
            result.m32 = a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32;
            result.m33 = a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33;

            return result;
        }

        public float this[int index]
        {
            get
            {
                return index switch
                {
                    0 => m00,
                    1 => m10,
                    2 => m20,
                    3 => m30,
                    4 => m01,
                    5 => m11,
                    6 => m21,
                    7 => m31,
                    8 => m02,
                    9 => m12,
                    10 => m22,
                    11 => m32,
                    12 => m03,
                    13 => m13,
                    14 => m23,
                    15 => m33,
                    _ => throw new IndexOutOfRangeException(),
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m00 = value;
                        break;
                    case 1:
                        m10 = value;
                        break;
                    case 2:
                        m20 = value;
                        break;
                    case 3:
                        m30 = value;
                        break;
                    case 4:
                        m01 = value;
                        break;
                    case 5:
                        m11 = value;
                        break;
                    case 6:
                        m21 = value;
                        break;
                    case 7:
                        m31 = value;
                        break;
                    case 8:
                        m02 = value;
                        break;
                    case 9:
                        m12 = value;
                        break;
                    case 10:
                        m22 = value;
                        break;
                    case 11:
                        m32 = value;
                        break;
                    case 12:
                        m03 = value;
                        break;
                    case 13:
                        m13 = value;
                        break;
                    case 14:
                        m23 = value;
                        break;
                    case 15:
                        m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        private void Set(CustomMatrix4x4 mat)
        {
            m00 = mat.m00;
            m01 = mat.m01;
            m02 = mat.m02;
            m03 = mat.m03;
            m10 = mat.m10;
            m11 = mat.m11;
            m12 = mat.m12;
            m13 = mat.m13;
            m20 = mat.m20;
            m21 = mat.m21;
            m22 = mat.m22;
            m23 = mat.m23;
            m30 = mat.m30;
            m31 = mat.m31;
            m32 = mat.m32;
            m33 = mat.m33;
        }

        public Vec3 MultiplyPoint(Vec3 point)
        {
            float x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
            float y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
            float z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
            float w = m30 * point.x + m31 * point.y + m32 * point.z + m33;

            if (Mathf.Abs(w) > Mathf.Epsilon)
                return new Vec3(x / w, y / w, z / w);

            return new Vec3(x, y, z);
        }

        /// <summary>
        /// Returns a matrix that is transformed by scale, rotation and position
        /// https://learnopengl.com/Getting-started/Transformations
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="rot"></param>
        /// <param name="sca"></param>
        /// <returns></returns>
        public static CustomMatrix4x4 TRS(Vec3 pos, CustomQuaternion rot, Vec3 sca)
        {
            var scale = Scale(sca);

            var rotation = Rotate(rot);

            var translation = Translate(pos);

            return translation * rotation * scale;
        }

        /// <summary>
        /// Returns a matrix that scales by the given vector.
        /// Saves the values in the diagonal of the matrix
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static CustomMatrix4x4 Scale(Vec3 vector)
        {
            var m = identity;

            m.m00 = vector.x;
            m.m11 = vector.y;
            m.m22 = vector.z;

            return m;
        }

        /// <summary>
        /// Returns a matrix that translates by the given vector.
        /// Saves the values at the last column of the matrix
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static CustomMatrix4x4 Translate(Vec3 vector)
        {
            var m = identity;

            m.m03 = vector.x;
            m.m13 = vector.y;
            m.m23 = vector.z;
            return m;
        }

        /// <summary>
        /// Returns a matrix that rotates by the given quaternion.
        /// https://ingmec.ual.es/~jlblanco/papers/jlblanco2010geometry3D_techrep.pdf
        /// (Page 18)
        /// q.w represents the angle of rotation (cos(angle/2))
        /// q.x, q.y and q.z represent the axis of rotation (a unit vector scaled by sin(angle/2)
        /// q.w = (cos(angle/2)
        /// q.x = x * sin(angle/2)
        /// q.y = y * sin(angle/2)
        /// Esta funcion representa una derivacion algebraica directa de la definicion fundamental de la rotacion de cuaterniones.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static CustomMatrix4x4 Rotate(CustomQuaternion q)
        {
            // Make it a pure rotation
            q.Normalize();

            #region Useful Vars
            var xx = q.x * q.x;
            var yy = q.y * q.y;
            var zz = q.z * q.z;
            var xy = q.x * q.y;
            var xz = q.x * q.z;
            var yz = q.y * q.z;
            var wx = q.w * q.x;
            var wy = q.w * q.y;
            var wz = q.w * q.z;
            var ww = q.w * q.w;
            #endregion

            var res = identity;

            /*
            // It uses only the 3x3 part of the matrix
            // El objetivo es encontrar una matriz R tal que, multiplicada por cualquier vector v,
            // el resultado sea el mismo que rotar v usando la formula de rotación de quaterniones
            // La forma de rotacion de un vector en R3 por un cuaternion unitario es:
            // v' = q * v * q^(-1) (q-1 siendo su conjugado)
            // se puede traducir en matrices al ser una rotacion lineal:
            // v' = R * v;
            */

            // Diagonales: Muestran que tanto se afectan los ejes de
            //             rotacion. Hay un + al lado del eje afectado.
            // 0 grados:
            // 1 + 0 - 0 - 0 = 1;
            // 1 - 0 + 0 - 0 = 1;
            // 1 - 0 - 0 + 0 = 1;
            // Cos(0) = 1;
            // 
            // Son los cosenos de los angulos entre los ejes originales y los rotados.
            // q = 0,1,0,0 (180 grados)
            // m00 = 0 + 1 - 0 - 0 = 1 -> cos(0°) = 1
            // m11 = 0 - 1 + 0 - 0 = -1 -> cos(180°) = -1
            // m22 = 0 - 1 - 0 + 0 = -1 -> cos(180°) = -1

            res.m00 = ww + xx - yy - zz;
            res.m11 = ww - xx + yy - zz;
            res.m22 = ww - xx - yy + zz;

            //Fuera de las diagonales: Representan el acoplamiento de los ejes debido a la rotacion.
            //x
            // Este en particular, cuantifica como la componente rotacional al rededor del eje Z acopla
            // los ejes X e Y
            res.m10 = 2 * (xy + wz); //x * y + w * z -> para modificar XY
            res.m20 = 2 * (xz - wy); //x * z

            //y
            res.m01 = 2 * (xy - wz); //y * x
            res.m21 = 2 * (yz + wx); //y * z

            //z
            res.m02 = 2 * (xz + wy); //x * z
            res.m12 = 2 * (yz - wx); //y * z

            return res;
        }

        private int ToIndex(int row, int column)
        {
            return column * 4 + row;
        }

        public float this[int row, int col]
        {
            get => this[ToIndex(row, col)];
            set => this[ToIndex(row, col)] = value;
        }

        /// <summary>
        /// https://dev.to/rk042/how-to-inverse-a-matrix-in-c-12jg
        /// </summary>
        /// <returns></returns>
        public CustomMatrix4x4 Inverse()
        {
            // Step 1: Matrix initialization
            // Copy 'this' into a working array of 4 rows (each row is 8 floats: 4 original + 4 identity)
            float[,] augmentedM = new float[4, 8];

            // Step 2: Augmented matrix setup
            for (int row = 0; row < 4; row++)
            {
                for (int cols = 0; cols < 4; cols++)
                    augmentedM[row, cols] = this[row, cols]; // assuming you have this[row,col] accessor

                for (int cols = 0; cols < 4; cols++)
                    augmentedM[row, cols + 4] = (row == cols) ? 1f : 0f; // identity on right
            }

            // Step 3: Gaussian elimination
            for (int pivotCol = 0; pivotCol < 4; pivotCol++)
            {
                // Pivot search: find row with largest abs value in column i
                int pivotRow = pivotCol;
                float maxVal = Mathf.Abs(augmentedM[pivotCol, pivotCol]);
                for (int row = pivotCol + 1; row < 4; row++)
                {
                    if (Mathf.Abs(augmentedM[row, pivotCol]) > maxVal)
                    {
                        maxVal = Mathf.Abs(augmentedM[row, pivotCol]);
                        pivotRow = row;
                    }
                }

                // If no valid pivot is found, matrix is not invertible (singular)
                if (Mathf.Approximately(maxVal, 0f))
                    return this;

                // Swap rows if needed
                if (pivotRow != pivotCol)
                {
                    for (int cols = 0; cols < 8; cols++)
                    {
                        float temp = augmentedM[pivotCol, cols];
                        augmentedM[pivotCol, cols] = augmentedM[pivotRow, cols];
                        augmentedM[pivotRow, cols] = temp;
                    }
                }

                // Step 4: Normalize pivot row
                float pivotVal = augmentedM[pivotCol, pivotCol];
                for (int cols = 0; cols < 8; cols++)
                    augmentedM[pivotCol, cols] /= pivotVal;

                // Step 5: Eliminate other rows
                for (int row = 0; row < 4; row++)
                {
                    if (row == pivotCol) continue;
                    float factor = augmentedM[row, pivotCol];
                    for (int cols = 0; cols < 8; cols++)
                        augmentedM[row, cols] -= factor * augmentedM[pivotCol, cols];
                }
            }

            // Step 6: Extract inverse (right half of augmented matrix)
            CustomMatrix4x4 inv = new CustomMatrix4x4();
            for (int row = 0; row < 4; row++)
            {
                for (int cols = 0; cols < 4; cols++)
                    inv[row, cols] = augmentedM[row, cols + 4];
            }

            return inv;
        }

    }
}