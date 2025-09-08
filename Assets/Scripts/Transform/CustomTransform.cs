using System;
using System.Collections;
using UnityEngine;
using static Unity.Mathematics.math;

namespace CustomMath
{
    public class CustomTransform : MonoBehaviour
    {
        public Vec3 localPosition;
        public CustomQuaternion localRotation;
        public Vec3 localScale = new Vec3(1, 1, 1);

        public CustomTransform parent;

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
                    localPosition = parent.localToWorldMatrix.MultiplyPoint(value) :
                    value;
            }
        }

        //public Vec3 eulerAngles
        //{

        //}

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

        //internal CustomTransform parentInternal
        //{
        //    get
        //    {

        //    }
        //    set
        //    {

        //    }
        //}

        public CustomMatrix4x4 worldToLocalMatrix
        {
            get
            {
                return localToWorldMatrix.InverseTRS(position, rotation, lossyScale);
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

        //public CustomTransform root => GetRoot();

        //public int childCount
        //{
        //    get
        //    {

        //    }
        //}

        public Vec3 lossyScale
        {
            get
            {
                return parent != null ? parent.lossyScale * localScale : localScale; 
            }
        }

        //public bool hasChanged
        //{
        //    get
        //    {

        //    }
        //    set
        //    {

        //    }
        //}


        //internal bool constrainProportionsScale
        //{
        //    get
        //    {
        //    }
        //    set
        //    {
        //    }
        //}

        //protected CustomTransform()
        //{
        //}

        //internal void SetLocalEulerHint(Vec3 euler)
        //{

        //}

        //private CustomTransform GetParent()
        //{

        //}

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
                var delta = worldT - parent.position;
                localPosition = parent.rotation.Inverse() * delta;
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

        //public void SetPositionAndRotation(Vec3 position, CustomQuaternion rotation)
        //{

        //}

        //public void SetLocalPositionAndRotation(Vec3 localPosition, CustomQuaternion localRotation)
        //{

        //}

        //public void GetPositionAndRotation(out Vec3 position, out CustomQuaternion rotation)
        //{

        //}

        //public void GetLocalPositionAndRotation(out Vec3 localPosition, out CustomQuaternion localRotation)
        //{

        //}

        //public void Translate(Vec3 translation, Space relativeTo = Space.Self)
        //{

        //}

        //public void Translate(Vec3 translation)
        //{
        //    Translate(translation, Space.Self);
        //}

        //public void Translate(float x, float y, float z, Space relativeTo = Space.Self)
        //{
        //    Translate(new Vec3(x, y, z), relativeTo);
        //}

        //public void Translate(float x, float y, float z)
        //{
        //    Translate(new Vec3(x, y, z), Space.Self);
        //}

        //public void Translate(Vec3 translation, CustomTransform relativeTo)
        //{
        //    if ((bool)relativeTo)
        //    {
        //        position += relativeTo.TransformDirection(translation);
        //    }
        //    else
        //    {
        //        position += translation;
        //    }
        //}

        //public void Translate(float x, float y, float z, CustomTransform relativeTo)
        //{
        //    Translate(new Vec3(x, y, z), relativeTo);
        //}

        //public void Rotate(Vec3 eulers, Space relativeTo = Space.Self)
        //{

        //}

        //public void Rotate(Vec3 eulers)
        //{
        //    Rotate(eulers, Space.Self);
        //}

        //public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
        //{
        //    Rotate(new Vec3(xAngle, yAngle, zAngle), relativeTo);
        //}

        //public void Rotate(float xAngle, float yAngle, float zAngle)
        //{
        //    Rotate(new Vec3(xAngle, yAngle, zAngle), Space.Self);
        //}

        //internal void RotateAroundInternal(Vec3 axis, float angle)
        //{

        //}

        //public void Rotate(Vec3 axis, float angle, Space relativeTo = Space.Self)
        //{

        //}

        //public void RotateAround(Vec3 point, Vec3 axis, float angle)
        //{

        //}

        //public void LookAt(CustomTransform target)
        //{
        //    if (target)
        //        LookAt(target.position, Vec3.up);
        //}

        //public void LookAt(Vec3 worldPosition, Vec3 worldUp)
        //{

        //}

        //public void LookAt(Vec3 worldPosition)
        //{

        //}

        //public Vec3 TransformDirection(Vec3 direction)
        //{

        //}

        //public void TransformDirections(ReadOnlySpan<Vec3> directions, Span<Vec3> transformedDirections)
        //{

        //}

        //public void TransformDirections(Span<Vec3> directions)
        //{

        //}

        //public Vec3 InverseTransformDirection(Vec3 direction)
        //{

        //}

        //public Vec3 InverseTransformDirection(float x, float y, float z)
        //{
        //    return InverseTransformDirection(new Vec3(x, y, z));
        //}

        //public void InverseTransformDirections(ReadOnlySpan<Vec3> directions, Span<Vec3> transformedDirections)
        //{

        //}

        //public void InverseTransformDirections(Span<Vec3> directions)
        //{

        //}

        //public Vec3 TransformVector(Vec3 vector)
        //{

        //}

        //public void TransformVectors(ReadOnlySpan<Vec3> vectors, Span<Vec3> transformedVectors)
        //{

        //}

        //public void TransformVectors(Span<Vec3> vectors)
        //{
        //    TransformVectorsInternal(vectors, vectors);
        //}

        //public Vec3 InverseTransformVector(Vec3 vector)
        //{
        //}


        //public void InverseTransformVectors(ReadOnlySpan<Vec3> vectors, Span<Vec3> transformedVectors)
        //{

        //}

        //public void InverseTransformVectors(Span<Vec3> vectors)
        //{
        //    InverseTransformVectorsInternal(vectors, vectors);
        //}

        public Vec3 TransformPoint(Vec3 position)
        {
            return localToWorldMatrix.MultiplyPoint(position);
        }


        //public void TransformPoints(ReadOnlySpan<Vec3> positions, Span<Vec3> transformedPositions)
        //{

        //}

        //public void TransformPoints(Span<Vec3> positions)
        //{
        //    TransformPoints(positions, positions);
        //}

        public Vec3 InverseTransformPoint(Vec3 position)
        {
            return worldToLocalMatrix.MultiplyPoint(position);
        }

        //public Vec3 InverseTransformPoint(float x, float y, float z)
        //{
        //    return InverseTransformPoint(new Vec3(x, y, z));
        //}


        //public void InverseTransformPoints(ReadOnlySpan<Vec3> positions, Span<Vec3> transformedPositions)
        //{

        //}

        //public void InverseTransformPoints(Span<Vec3> positions)
        //{
        //    InverseTransformPoints(positions, positions);
        //}

        //private CustomTransform GetRoot()
        //{

        //}

        //public void DetachChildren()
        //{

        //}

        //public void SetSiblingIndex(int index)
        //{

        //}

        //internal void MoveAfterSibling(CustomTransform CustomTransform, bool notifyEditorAndMarkDirty)
        //{

        //}

        //public int GetSiblingIndex()
        //{

        //}

        //private unsafe CustomTransform FindRelativeTransformWithPath(string path, bool isActiveOnly = false)
        //{

        //}

        //public CustomTransform Find(string n)
        //{
        //    if (n == null)
        //    {
        //        throw new ArgumentNullException("Name cannot be null");
        //    }

        //    return FindRelativeTransformWithPath(n, isActiveOnly: false);
        //}

        //internal void SendTransformChangedScale()
        //{

        //}

        //public bool IsChildOf(CustomTransform parent)
        //{

        //}

        //public IEnumerator GetEnumerator()
        //{
        //    return new Enumerator(this);
        //}

        //public CustomTransform GetChild(int index)
        //{

        //}

        //internal bool IsNonUniformScaleTransform()
        //{

        //}

        //private void SetConstrainProportionsScale(bool isLinked)
        //{

        //}

        //private bool IsConstrainProportionsScale()
        //{

        //}
    }

}

