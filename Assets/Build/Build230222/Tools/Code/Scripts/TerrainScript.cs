using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{

    [SerializeField]
    private float width;
    [SerializeField]
    private float breadth;
    [SerializeField]
    [Tooltip("Octaves are the number of layers of noise you apply to the terrain.")]
    private float octave = 8;
    [SerializeField]
    [Tooltip("The amount that frequecny will increase by per octave.")]
    private float lacunarity = 2.0f;
    [SerializeField]
    [Tooltip("Gain is actually the amount amplitude with drop by per octave.")]
    private float gain = 0.4f;
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private float amplitude = 10.0f;
    [SerializeField]
    private float frequency = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //@brief Public getters and setters.
    public void SetWidthAndBreadth(float _width, float _breadth)
    {
        width = _width;
        breadth = _breadth;
    }

    public void SetOctaves(int octaves)
    {
        octave = octaves;
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetBreadth()
    {
        return breadth;
    }

    public void Noise(float lacunarity, float gain, float offsetX, float offsetZ)
    {
        Vector3[] vertices = mesh.vertices;
        float amp = amplitude;
        float freq = frequency;

        for (int o = 0; o < octave; ++o)
        {
            for (int x = 0; x < width; ++x)
            {
                for(int z = 0; z < breadth; ++z)
                {
                    int index = (x * (int)width) + z;
                    vertices[index].y += Mathf.PerlinNoise((float)x + offsetX, (float)z + offsetZ) * amp;
                }
            }

            freq *= lacunarity;
            amp *= gain;

        }
        mesh.SetVertices(vertices);
    }

}
