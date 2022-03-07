using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{

    [SerializeField]
    private int width;
    [SerializeField]
    private int breadth;

    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private MeshFilter meshFilter;

    //Mesh Data
    [SerializeField]
    private Vector3[] vertices;
    [SerializeField]
    private Vector3[] normal;
    [SerializeField]
    private Vector2[] uvs;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMesh()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0f, 0f, 0f),
            new Vector3(width, 0f, 0f),
            new Vector3(0f, 0f, breadth),
            new Vector3(width, 0f, breadth)
        };

        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            //Lower left tri
            0, 2, 1,
            //Upper right tri
            2, 3, 1
        };

        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };

        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        mesh.uv = uv;

        //Apply the geometric data.
        meshFilter.mesh = mesh;
    }


    public void Reset()
    {

    }


}
