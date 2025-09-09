using UnityEngine;

namespace CustomMath
{
    /// <summary>
    /// https://developer.unigine.com/en/docs/latest/code/fundamentals/matrix_transformations/index?implementationLanguage=cpp
    /// </summary>
    public class CustomTransform : MonoBehaviour
    {
        public Vec3 localPosition;
        public Vec3 eulerRotation;
        public Vec3 localScale = new Vec3(1, 1, 1);
        public CustomQuaternion localRotation;

        public CustomTransform parent;

        Transform t;

        private void Awake()
        {
            localRotation = CustomQuaternion.Euler(eulerRotation);
        }

        private void OnValidate()
        {
            localRotation = CustomQuaternion.Euler(eulerRotation);

        }

        public Vec3 position
        {
            get
            {
                return parent != null ?
                    parent.localToWorldMatrix.MultiplyPoint(localPosition) :
                    localPosition;
            }
            set
            {
                localPosition = parent != null ?
                    parent.worldToLocalMatrix.MultiplyPoint(value) :
                    value;
            }
        }

        public Vec3 localEulerAngles
        {
            get
            {
                return localRotation.eulerAngles;
            }
            set
            {
                localRotation = CustomQuaternion.Euler(value);
            }
        }

        public Vec3 right
        {
            get
            {
                return rotation * Vec3.right;
            }
            set
            {
                rotation = CustomQuaternion.FromToRotation(Vec3.right, value);
            }
        }

        public Vec3 up
        {
            get
            {
                return rotation * Vec3.up;
            }
            set
            {
                rotation = CustomQuaternion.FromToRotation(Vec3.up, value);
            }
        }

        public Vec3 forward
        {
            get
            {
                return rotation * Vec3.forward;
            }
            set
            {
                rotation = CustomQuaternion.LookRotation(value);
            }
        }

        public CustomQuaternion rotation
        {
            get
            {
                return parent != null ?
                    parent.rotation * localRotation : localRotation;
            }
            set
            {
                localRotation = parent != null ?
                    parent.rotation.Inverse() * value : value;
            }
        }


        public CustomMatrix4x4 worldToLocalMatrix
        {
            get
            {
                var S = CustomMatrix4x4.Scale(localScale);
                var R = CustomMatrix4x4.Rotate(localRotation);
                var T = CustomMatrix4x4.Translate(localPosition);

                var local = T * R * S;

                return local;
            }
        }

        public CustomMatrix4x4 localToWorldMatrix
        {
            get
            {
                return parent != null ? parent.localToWorldMatrix * worldToLocalMatrix : worldToLocalMatrix;
            }
        }

        public Vec3 lossyScale
        {
            get
            {
                return parent != null ? parent.lossyScale * localScale : localScale;
            }
        }

        /// <summary>
        /// https://learnopengl.com/Getting-started/Transformations
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        public void SetParent(CustomTransform parent, bool worldPositionStays = true)
        {
            if (this.parent == parent)
                return;

            var worldT = position;
            var worldR = rotation;
            var worldS = lossyScale;

            if (!worldPositionStays)
            {
                this.parent = parent;
                return;
            }

            CustomMatrix4x4 worldMat = CustomMatrix4x4.TRS(worldT, worldR, worldS);

            CustomMatrix4x4 localMat;

            if (parent)
            {
                localMat = parent.worldToLocalMatrix * worldToLocalMatrix;
            }
            else
            {
                localMat = worldMat;
            }

            localPosition = localMat.GetPosition();
            localRotation = localMat.rotation;
            localScale = localMat.lossyScale;

            this.parent = parent;
        }

        public Vec3 TransformPoint(Vec3 position)
        {
            return localToWorldMatrix.MultiplyPoint(position);
        }

        public Vec3 InverseTransformPoint(Vec3 position)
        {
            return worldToLocalMatrix.MultiplyPoint(position);
        }
    }

}

