using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TerrainMesh : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer meshRenderer = null;
    [SerializeField]
    private MeshFilter meshFilter = null;

    [SerializeField]
    private Texture2D heightMapTexture;

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

    private int resolution = 64;
    private int sizeX = 1000;
    private int sizeZ = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BakeHeightMap()
    {
        heightMapTexture = new Texture2D(512, 512, TextureFormat.RGB24, true);
        for(int x = 0; x < 512; ++x)
        {
            for(int y = 0; y < 512; ++y)
            {
                Color value = new Color(0, 0, 0, 0);
                float noise = PerlinNoise(x, y);
                Debug.Log("Noise " + noise);
                value.r = noise; 
                value.b = noise;
                value.g = noise;
                heightMapTexture.SetPixel(x, y, value);
            }
        }

        byte[] bytes = heightMapTexture.EncodeToPNG();

        var dirPath = Application.dataPath + "/../HeightMaps/";
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "HeightMap" + ".png", bytes);
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
        meshRenderer.sharedMaterial = new Material(Shader.Find("Unlit/TerrainShader"));
        meshFilter = gameObject.AddComponent<MeshFilter>();
       
        Mesh mesh = new Mesh();
        //Calculate positions.
        List<Vector3> vertices = new List<Vector3>();
        for (int x = 0; x < resolution; ++x)
        {
            for(int z = 0; z < resolution; ++z)
            { 
                vertices.Add(new Vector3(x * ((float)sizeX / (float)resolution), 0f + PerlinNoise(x, z), z * ((float)sizeZ / (float)resolution)));
            }
        }

        mesh.vertices = vertices.ToArray();

        List<int> indices = new List<int>();
        //Calculate indices for each polygon.

        for(int x = 0; x < resolution - 1; ++x)
        {
            for(int z = 0; z < resolution - 1; ++z)
            {
                indices.Add((x * resolution) + z);
                indices.Add((x * resolution) + z + resolution);
                indices.Add((x * resolution) + z + 1);

                indices.Add((x * resolution) + z + resolution);
                indices.Add((x * resolution) + z + resolution + 1);
                indices.Add((x * resolution) + z + 1);
            }
        }

        mesh.triangles = indices.ToArray();

        //Calculate normals.
        List<Vector3> normals = new List<Vector3>();

        for (int x = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z)
            {
                normals.Add(new Vector3(0, 1, 0));
            }
        }

        mesh.normals = normals.ToArray();
        mesh.RecalculateNormals();


        //Calculate texture coordinates.
        List<Vector2> uv = new List<Vector2>();

        for (int x = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z)
            {
                uv.Add(new Vector2((float)x / (float)resolution, (float)z / (float)resolution));
            }
        }

        mesh.uv = uv.ToArray();

        //Apply the geometric data.
        meshFilter.mesh = mesh;
    }


    public void Reset()
    { 
    }

    public void SetResolution(int terrrainDimension) => resolution = terrrainDimension;
    public void SetAmplitude(float amp) => amplitude = amp;
    public void SetFrequency(float freq) => frequency = freq;
    public void SetOffsetU(float oU) => offsetU = oU;
    public void SetOffsetV(float oV) => offsetV = oV;
    public void SetLacunarity(float lac) => lacunarity = lac;
    public void SetLoss(float los) => loss = los;

    public int GetResolution() => resolution;
    public float GetAmplitude() => amplitude;
    public float GetFrequency() => frequency;
    public float GetLacunarity() => lacunarity;
    public float GetLoss() => loss;
    public float GetOffsetU() => offsetU;
    public float GetOffsetV() => offsetV;
}
