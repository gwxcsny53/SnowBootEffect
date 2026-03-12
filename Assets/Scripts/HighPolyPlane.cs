using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HighPolyPlane : MonoBehaviour
{
    [Tooltip("雪地大小（米）")]
    public float size = 10f;
    [Tooltip("网格密度，数字越大坑越圆滑，但不要超过 200")]
    public int segments = 100;

    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "HighPolyPlane";

        int numVertices = (segments + 1) * (segments + 1);
        Vector3[] vertices = new Vector3[numVertices];
        Vector2[] uvs = new Vector2[numVertices];
        int[] triangles = new int[segments * segments * 6];

        float step = size / segments;
        float halfSize = size / 2f;

        // 生成密集的顶点
        int v = 0;
        for (int z = 0; z <= segments; z++)
        {
            for (int x = 0; x <= segments; x++)
            {
                vertices[v] = new Vector3(x * step - halfSize, 0, z * step - halfSize);
                uvs[v] = new Vector2((float)x / segments, (float)z / segments);
                v++;
            }
        }

        // 缝合三角形
        int t = 0;
        for (int z = 0; z < segments; z++)
        {
            for (int x = 0; x < segments; x++)
            {
                int i = x + z * (segments + 1);
                triangles[t++] = i;
                triangles[t++] = i + segments + 1;
                triangles[t++] = i + 1;

                triangles[t++] = i + 1;
                triangles[t++] = i + segments + 1;
                triangles[t++] = i + segments + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // 自动计算法线

        GetComponent<MeshFilter>().mesh = mesh;
    }
}