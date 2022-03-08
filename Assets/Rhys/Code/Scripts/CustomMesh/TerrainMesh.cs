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

    private int size = 1;
    private int resolution = 256;

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

        int terrainDimension = size * size;

        //Calculate positions.
        Vector3[] vertices = new Vector3[terrainDimension];

        for (int x = 0; x < size; ++x)
        {
            for(int z = 0; z < size; ++z)
            {
                vertices[(size * z) + x] = new Vector3(x, 0f, z);
            }
        }

        mesh.vertices = vertices;

        //Calculate indices for each polygon.
        int[] tris = new int[6]
        {
            //Lower left tri
            0, 2, 1,
            //Upper right tri
            2, 3, 1
        };

        for(int x = 0; x < size; ++x)
        {
            for(int z = 0; z < size; ++z)
            {
                
            }
        }

        mesh.triangles = tris;

        //Calculate normals.
        Vector3[] normals = new Vector3[terrainDimension];

        mesh.normals = normals;

        //Calculate texture coordinates.
        Vector2[] uv = new Vector2[terrainDimension];

        mesh.uv = uv;

        //Apply the geometric data.
        meshFilter.mesh = mesh;
    }


    public void Reset()
    { 
    }

    public void SetTerrainSize(int terrrainDimension) => size = terrrainDimension;

}
