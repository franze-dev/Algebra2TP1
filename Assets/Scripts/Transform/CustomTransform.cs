using System;
using System.Collections;
using UnityEngine;
using static Unity.Mathematics.math;

public class CustomTransform : MonoBehaviour
{
    private class Enumerator : IEnumerator
    {
        private Transform outer;

        private int currentIndex = -1;

        public object Current => outer.GetChild(currentIndex);

        internal Enumerator(Transform outer)
        {
            this.outer = outer;
        }

        public bool MoveNext()
        {
            int childCount = outer.childCount;
            return ++currentIndex < childCount;
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }

    public Vector3 position
    {
        get
        {
            
        }
        set
        {
            
        }
    }

    public Vector3 localPosition
    {
        get
        {
            
        }
        set
        {
            
        }
    }

    public Vector3 eulerAngles
    {
       
    }

    public Vector3 localEulerAngles
    {
        get
        {
            return localRotation.eulerAngles;
        }
        set
        {
            localRotation = Quaternion.Euler(value);
        }
    }

    public Vector3 right
    {
        get
        {
            return rotation * Vector3.right;
        }
        set
        {
            rotation = Quaternion.FromToRotation(Vector3.right, value);
        }
    }

    public Vector3 up
    {
        get
        {
            return rotation * Vector3.up;
        }
        set
        {
            rotation = Quaternion.FromToRotation(Vector3.up, value);
        }
    }

    public Vector3 forward
    {
        get
        {
            return rotation * Vector3.forward;
        }
        set
        {
            rotation = Quaternion.LookRotation(value);
        }
    }

    public Quaternion rotation
    {
        get
        {

        }
        set
        {
            
        }
    }

    //
    // Resumen:
    //     The rotation of the transform relative to the transform rotation of the parent.
    public Quaternion localRotation
    {
        get
        {
            
        }
        set
        {
            
        }
    }

    internal RotationOrder rotationOrder
    {
        get
        {

        }
        set
        {

        }
    }

    public Vector3 localScale
    {
        get
        {

        }
        set
        {

        }
    }

    //
    // Resumen:
    //     The parent of the transform.
    public Transform parent
    {
        get
        {

        }
        set
        {

        }
    }

    internal Transform parentInternal
    {
        get
        {

        }
        set
        {

        }
    }

    public Matrix4x4 worldToLocalMatrix
    {
        get
        {

        }
    }

    public Matrix4x4 localToWorldMatrix
    {
        get
        {
            
        }
    }

    public Transform root => GetRoot();

    public int childCount
    {
        get
        {

        }
    }

    //
    // Resumen:
    //     The global scale of the object (Read Only).
    public Vector3 lossyScale
    {
        get
        {
            
        }
    }

    public bool hasChanged
    {
        get
        {
            
        }
        set
        {
            
        }
    }

    //
    // Resumen:
    //     The transform capacity of the transform's hierarchy data structure.
    public int hierarchyCapacity
    {
        get
        {
        }
        set
        {
        }
    }

    public int hierarchyCount => ;

    internal bool constrainProportionsScale
    {
        get
        {
        }
        set
        {
        }
    }

    protected Transform()
    {
    }

    internal Vector3 GetLocalEulerAngles(RotationOrder order)
    {

    }

    internal void SetLocalEulerAngles(Vector3 euler, RotationOrder order)
    {

    }

    internal void SetLocalEulerHint(Vector3 euler)
    {

    }

    internal int GetRotationOrderInternal()
    {

    }

    internal void SetRotationOrderInternal(RotationOrder rotationOrder)
    {

    }

    private Transform GetParent()
    {

    }

    public void SetParent(Transform p)
    {
        SetParent(p, true);
    }

    public void SetParent(Transform parent, bool worldPositionStays)
    {
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
    {

    }

    public void SetLocalPositionAndRotation(Vector3 localPosition, Quaternion localRotation)
    {

    }

    public void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
    {
        
    }

    public void GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation)
    {
        
    }

    public void Translate(Vector3 translation, Space relativeTo = Space.Self)
    {
        
    }

    public void Translate(Vector3 translation)
    {
        Translate(translation, Space.Self);
    }

    public void Translate(float x, float y, float z, Space relativeTo = Space.Self)
    {
        Translate(new Vector3(x, y, z), relativeTo);
    }

    public void Translate(float x, float y, float z)
    {
        Translate(new Vector3(x, y, z), Space.Self);
    }

    public void Translate(Vector3 translation, Transform relativeTo)
    {
        if ((bool)relativeTo)
        {
            position += relativeTo.TransformDirection(translation);
        }
        else
        {
            position += translation;
        }
    }

    public void Translate(float x, float y, float z, Transform relativeTo)
    {
        Translate(new Vector3(x, y, z), relativeTo);
    }

    public void Rotate(Vector3 eulers, Space relativeTo = Space.Self)
    {
        
    }

    public void Rotate(Vector3 eulers)
    {
        Rotate(eulers, Space.Self);
    }

    public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self)
    {
        Rotate(new Vector3(xAngle, yAngle, zAngle), relativeTo);
    }

    public void Rotate(float xAngle, float yAngle, float zAngle)
    {
        Rotate(new Vector3(xAngle, yAngle, zAngle), Space.Self);
    }

    internal void RotateAroundInternal(Vector3 axis, float angle)
    {
        
    }

    public void Rotate(Vector3 axis, float angle, Space relativeTo = Space.Self)
    {

    }

    public void RotateAround(Vector3 point, Vector3 axis, float angle)
    {
        
    }

    public void LookAt(Transform target)
    {
        if (target)
            LookAt(target.position, Vector3.up);
    }

    public void LookAt(Vector3 worldPosition, Vector3 worldUp)
    {

    }

    public void LookAt(Vector3 worldPosition)
    {

    }

    public Vector3 TransformDirection(Vector3 direction)
    {

    }

    public void TransformDirections(ReadOnlySpan<Vector3> directions, Span<Vector3> transformedDirections)
    {
        
    }

    public void TransformDirections(Span<Vector3> directions)
    {

    }

    public Vector3 InverseTransformDirection(Vector3 direction)
    {
        
    }

    public Vector3 InverseTransformDirection(float x, float y, float z)
    {
        return InverseTransformDirection(new Vector3(x, y, z));
    }

    public void InverseTransformDirections(ReadOnlySpan<Vector3> directions, Span<Vector3> transformedDirections)
    {
        
    }

    public void InverseTransformDirections(Span<Vector3> directions)
    {

    }

    public Vector3 TransformVector(Vector3 vector)
    {

    }

    public void TransformVectors(ReadOnlySpan<Vector3> vectors, Span<Vector3> transformedVectors)
    {

    }

    public void TransformVectors(Span<Vector3> vectors)
    {
        TransformVectorsInternal(vectors, vectors);
    }

    public Vector3 InverseTransformVector(Vector3 vector)
    {
    }


    public void InverseTransformVectors(ReadOnlySpan<Vector3> vectors, Span<Vector3> transformedVectors)
    {

    }

    public void InverseTransformVectors(Span<Vector3> vectors)
    {
        InverseTransformVectorsInternal(vectors, vectors);
    }

    public Vector3 TransformPoint(Vector3 position)
    {

    }


    public void TransformPoints(ReadOnlySpan<Vector3> positions, Span<Vector3> transformedPositions)
    {

    }

    public void TransformPoints(Span<Vector3> positions)
    {
        TransformPoints(positions, positions);
    }

    public Vector3 InverseTransformPoint(Vector3 position)
    {

    }

    public Vector3 InverseTransformPoint(float x, float y, float z)
    {
        return InverseTransformPoint(new Vector3(x, y, z));
    }


    public void InverseTransformPoints(ReadOnlySpan<Vector3> positions, Span<Vector3> transformedPositions)
    {

    }

    public void InverseTransformPoints(Span<Vector3> positions)
    {
        InverseTransformPoints(positions, positions);
    }

    private Transform GetRoot()
    {

    }

    public void DetachChildren()
    {

    }

    public void SetAsFirstSibling()
    {

    }

    public void SetAsLastSibling()
    {

    }

    public void SetSiblingIndex(int index)
    {

    }

    internal void MoveAfterSibling(Transform transform, bool notifyEditorAndMarkDirty)
    {
       
    }

    public int GetSiblingIndex()
    {

    }

    private unsafe Transform FindRelativeTransformWithPath(string path, bool isActiveOnly = false)
    {
        
    }

    public Transform Find(string n)
    {
        if (n == null)
        {
            throw new ArgumentNullException("Name cannot be null");
        }

        return FindRelativeTransformWithPath(n, isActiveOnly: false);
    }

    internal void SendTransformChangedScale()
    {

    }

    public bool IsChildOf(Transform parent)
    {
        
    }

    public IEnumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    public Transform GetChild(int index)
    {

    }

    internal bool IsNonUniformScaleTransform()
    {

    }

    private void SetConstrainProportionsScale(bool isLinked)
    {
        
    }

    private bool IsConstrainProportionsScale()
    {
        
    }
}
