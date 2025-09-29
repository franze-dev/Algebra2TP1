using UnityEngine;

namespace CustomMath
{
    /// <summary>
    /// https://developer.unigine.com/en/docs/latest/code/fundamentals/matrix_transformations/index?implementationLanguage=cpp
    /// https://ingmec.ual.es/~jlblanco/papers/jlblanco2010geometry3D_techrep.pdf
    /// https://learnopengl.com/Getting-started/Transformations
    /// </summary>
    public class CustomTransform : MonoBehaviour
    {
        #region Variables
        public Vec3 localPosition;
        public Vec3 eulerRotation;
        public Vec3 localScale = new Vec3(1, 1, 1);
        public CustomQuaternion localRotation;
        public CustomTransform parent;
        #endregion

        #region Mono
        private void Awake()
        {
            // To get the local rotation I give it the yaw pitch and roll (euler) angles and 
            // transform it to a quaternion rotation.
            localRotation = CustomQuaternion.Euler(eulerRotation);

            // If it has a parent that has a CustomTransform, automatically assign it as parent.
            if (parent == null && transform.parent != null)
            {
                var parentTransform = transform.parent.GetComponent<CustomTransform>();
                if (parentTransform != null)
                    SetParent(parentTransform);
            }

            //If the original transform of this object is not zero, then assign it to the local position
            if (transform.position != Vec3.zero)
                localPosition = new(transform.position);
        }

        private void OnValidate()
        {
            localRotation = CustomQuaternion.Euler(eulerRotation);
        }
        #endregion

        #region Properties

        /// <summary>
        /// The world space position
        /// </summary>
        public Vec3 position
        {
            get
            {
                // Multiplies the local position by the matrix that transforms it to world position
                // according to its parent. If it has no parent, it just returns the local position.
                return parent != null ?
                    parent.localToWorldMatrix.MultiplyPoint(localPosition) :
                    localPosition;
            }
            set
            {
                // Sets the value trasformed into local position according to its parent or just
                // assigns it if it has no parent.
                localPosition = parent != null ?
                    parent.worldToLocalMatrix.MultiplyPoint(value) :
                    value;
            }
        }

        /// <summary>
        /// The world space rotation
        /// </summary>
        public CustomQuaternion rotation
        {
            get
            {
                // Returns the local rotation, modified by the parent's rotation or not.
                return parent != null ?
                    parent.rotation * localRotation : localRotation;
            }
            set
            {
                // https://danceswithcode.net/engineeringnotes/quaternions/quaternions.html
                // If you isolate the localRotation from the equation above, this is the result you get (the inverse part)
                // woldR = parentR * localR
                // Since in quaternions there's no division, I cant do lR = wR/pR, so instead, I have to use the inverse
                // of the parent rotation. So:
                // pR-1 * wR = pR-1 * (pR * lR)
                // Quaternion multiplication is associative, so I can regroup the values like:
                // pR-1 * wR = (pR-1 * pR) * lR
                // Knowing what pR-1 * pR results in an identity quaternion, then I finally have:
                // pR-1 * wR = lR 
                localRotation = parent != null ?
                    parent.rotation.Inverse() * value : value;
            }
        }


        public CustomMatrix4x4 worldToLocalMatrix
        {
            get
            {
                //it inverts te localToWorldMatrix, so it gets the worldToLocalMatrix.
                return localToWorldMatrix.Inverse();
            }
        }

        /// <summary>
        /// Convert a point from local space to world space.
        /// </summary>
        public CustomMatrix4x4 localToWorldMatrix
        {
            get
            {
                // Gets the TRS matrix of the local values. Multiplies them by the parent's own local to world matrix
                // or just returns it. If you multiply it by a point, it will transform it according to those values.
                var local = CustomMatrix4x4.TRS(localPosition, localRotation, localScale);
                return parent != null ? parent.localToWorldMatrix * local : local;
            }
        }

        /// <summary>
        /// The lossy world scale
        /// </summary>
        public Vec3 lossyScale
        {
            get
            {
                return parent != null ? parent.lossyScale * localScale : localScale;
            }
        }

        #endregion

        #region Funcs
/// <summary>
        /// Sets the parent of this transform.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        public void SetParent(CustomTransform parent, bool worldPositionStays = true)
        {
            // The world position would stay if you assign all the local values to the world
            // values.
            if (worldPositionStays)
            {
                localPosition = position;
                localRotation = rotation;
                localScale = lossyScale;
            }

            this.parent = parent;
        }
        #endregion
    }

}

