using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMesh : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private MeshFilter meshFilter = null;

    //Mesh Data
    [SerializeField]
    private List<float> heightMap;

    private float height = 0f;
    private float loss = 0.5f;
    private float lacunarity = 2f;
    private float amplitude = 50.0f;
    private float frequency = 0.01f;
    private float offsetU = 0f;
    private float offsetV = 0f;

    private int size = 64;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float PerlinNoise(float x, float z)
    {
        float a = amplitude;
        float f = frequency;
        float h = 0f;

        for (int o = 0; o < 8; ++o)
        {
            h += Mathf.PerlinNoise((x + offsetU) * f, (z + offsetV) * f) * a;
            a *= 0.5f;
            f *= 2f;
        }

        h = 1f - h; 

        return h;
    }

    public void GenerateMesh()
    {
        DestroyImmediate(meshRenderer);
        DestroyImmediate(meshFilter);
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        meshFilter = gameObject.AddComponent<MeshFilter>();
       
        Mesh mesh = new Mesh();
        //Calculate positions.
        List<Vector3> vertices = new List<Vector3>();
        for (int x = 0; x < size; ++x)
        {
            for(int z = 0; z < size; ++z)
            { 
                vertices.Add(new Vector3(x, 0f + PerlinNoise(x, z), z));
            }
        }

        mesh.vertices = vertices.ToArray();

        List<int> indices = new List<int>();
        //Calculate indices for each polygon.

        for(int x = 0; x < size - 1; ++x)
        {
            for(int z = 0; z < size - 1; ++z)
            {
                indices.Add((x * size) + z);
                indices.Add((x * size) + z + size);
                indices.Add((x * size) + z + 1);

                indices.Add((x * size) + z + size);
                indices.Add((x * size) + z + size + 1);
                indices.Add((x * size) + z + 1);
            }
        }

        mesh.triangles = indices.ToArray();

        //Calculate normals.
        List<Vector3> normals = new List<Vector3>();

        for (int x = 0; x < size; ++x)
        {
            for (int z = 0; z < size; ++z)
            {
                normals.Add(new Vector3(0, 1, 0));
            }
        }

        mesh.normals = normals.ToArray();
        mesh.RecalculateNormals();


        //Calculate texture coordinates.
        List<Vector2> uv = new List<Vector2>();

        for (int x = 0; x < size; ++x)
        {
            for (int z = 0; z < size; ++z)
            {
                uv.Add(new Vector2((float)x / (float)size, (float)z / (float)size));
            }
        }

        mesh.uv = uv.ToArray();

        //Apply the geometric data.
        meshFilter.mesh = mesh;
    }


    public void Reset()
    { 
    }

    public void SetTerrainSize(int terrrainDimension) => size = terrrainDimension;
    public void SetAmplitude(float amp) => amplitude = amp;
    public void SetFrequency(float freq) => frequency = freq;
    public void SetOffsetU(float oU) => offsetU = oU;
    public void SetOffsetV(float oV) => offsetV = oV;
    public void SetLacunarity(float lac) => lacunarity = lac;
    public void SetLoss(float los) => loss = los;

    public int GetTerrainSize() => size;
    public float GetAmplitude() => amplitude;
    public float GetFrequency() => frequency;
    public float GetLacunarity() => lacunarity;
    public float GetLoss() => loss;
    public float GetOffsetU() => offsetU;
    public float GetOffsetV() => offsetV;
}
