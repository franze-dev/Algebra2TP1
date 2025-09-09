using UnityEngine;
using CustomMath;
using System;

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

        Gizmos.DrawMesh(mesh, (Vector3)worldPos, unityRot, (Vector3)worldScale);
    }
}
