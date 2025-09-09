using UnityEngine;

namespace CustomMath
{
    public class CustomTransform : MonoBehaviour
    {
        public Vec3 localPosition;
        public Vec3 eulerRotation;
        public Vec3 localScale = new Vec3(1, 1, 1);
        public CustomQuaternion localRotation;

        public CustomTransform parent;

        private void Awake()
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
                return parent != null ? parent.localToWorldMatrix.Inverse() : CustomMatrix4x4.identity;
            }
        }

        public CustomMatrix4x4 localToWorldMatrix
        {
            get
            {
                var T = CustomMatrix4x4.Translate(localPosition);
                var R = CustomMatrix4x4.Rotate(localRotation);
                var S = CustomMatrix4x4.Scale(localScale);

                var local = T * R * S;

                return parent != null ? parent.localToWorldMatrix * local : local;
            }
        }

        public Vec3 lossyScale
        {
            get
            {
                return parent != null ? parent.lossyScale * localScale : localScale;
            }
        }

        public void SetParent(CustomTransform parent, bool worldPositionStays = true)
        {
            this.parent = parent;

            if (!worldPositionStays)
                return;

            Vec3 worldT = position;
            var worldR = rotation;
            Vec3 worldS = lossyScale;

            if (parent)
            {
                localPosition = parent.worldToLocalMatrix.MultiplyPoint(worldT);
                localRotation = parent.rotation.Inverse() * worldR;
                localScale = worldS / parent.lossyScale;
            }
            else
            {
                localPosition = worldT;
                localRotation = worldR;
                localScale = worldS;
            }
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

