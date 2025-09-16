using UnityEngine;
using CustomMath;

public class CustomMathTest : MonoBehaviour
{
    public Mesh cubeMesh;
    public Mesh sphereMesh;
    public Mesh capsuleMesh;

    [SerializeField] private CustomTransform root;
    [SerializeField] private CustomTransform child;
    [SerializeField] private CustomTransform grandchild;

    private void Start()
    {
        if (root == null || child == null || grandchild == null)
            Debug.LogError("Transforms are null");

        child.SetParent(root);

        grandchild.SetParent(child);
    }

    private void OnDrawGizmos()
    {
        if (root == null || child == null || grandchild == null)
            return;

        DrawMesh(root, cubeMesh, Color.red);
        DrawMesh(child, sphereMesh, Color.green);
        DrawMesh(grandchild, capsuleMesh, Color.blue);
    }

    private void DrawMesh(CustomTransform t, Mesh mesh, Color color)
    {
        if (color != null)
            Gizmos.color = color;
        else
            Gizmos.color = Color.white;

        Vec3 worldPos = t.position;

        Vec3 worldScale = t.lossyScale;

        var worldRot = t.rotation;

        var unityRot = new Quaternion(worldRot.x, worldRot.y, worldRot.z, worldRot.w);

        //Gizmos.matrix = ToUnity(t.worldToLocalMatrix);

        Gizmos.DrawMesh(mesh, (Vector3)worldPos, unityRot, (Vector3)worldScale);
    }

    private Matrix4x4 ToUnity(CustomMatrix4x4 m)
    {
        Matrix4x4 result = new Matrix4x4();

        result.m00 = m.m00;
        result.m01 = m.m01; 
        result.m02 = m.m02; 
        result.m03 = m.m03;

        result.m10 = m.m10;
        result.m11 = m.m11;
        result.m12 = m.m12;
        result.m13 = m.m13;

        result.m20 = m.m20;
        result.m21 = m.m21;
        result.m22 = m.m22;
        result.m23 = m.m23;

        result.m30 = m.m30;
        result.m31 = m.m31;
        result.m32 = m.m32;
        result.m33 = m.m33;

        return result;
    }
}
