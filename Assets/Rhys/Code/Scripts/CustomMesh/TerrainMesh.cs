using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

public class TerrainMesh : MonoBehaviour
{
    //Base Terrain Attributes
    private MeshRenderer meshRenderer = null;
    private MeshFilter meshFilter = null;
    private Texture2D heightMapTexture;
    private List<Vector3> vertices;
    //Mesh Data
    private List<float> heightMap;

    //Perlin noise attributes
    private float loss = 0.5f;
    private float lacunarity = 2f;
    private float amplitude = 50.0f;
    private float frequency = 0.01f;
    private float offsetU = 0f;
    private float offsetV = 0f;

    //Level of detail
    private int resolution = 64;
    private int size = 128;
    private int scale = 100;


    //////////////////////////////////////////////////////
    //BASE TERRAIN CODE
    //////////////////////////////////////////////////////

    public void BakeHeightMap()
    {
        heightMapTexture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
        float[] hm = heightMap.ToArray();
        Color texture = new Color();
        for(int x = 0; x < resolution; ++x)
        {
            for(int y = 0; y < resolution; ++y)
            {
                //One array access per iteration.
                float height = hm[(x * resolution) + y];
                texture.r = height;
                texture.g = height;
                texture.b = height;
                texture.a = 1f;
                heightMapTexture.SetPixel(x, y, texture);
            }
        }

        byte[] bytes = heightMapTexture.EncodeToPNG();

        var dirPath = Application.dataPath + "/Rhys/Textures/";
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        File.WriteAllBytes(dirPath + "HeightMap" + ".png", bytes);
    }

    public void LoadFromFile(string filePath)
    {
        if(filePath == " ")
        {
            Debug.LogError("Invalid filepath");
        }
        else
        {
            
            FileStream fs = File.OpenRead(filePath);
            byte[] data = new byte[256];
            fs.Read(data, 0, 256);
            heightMap.Clear();
            for(int i = 0; i < data.Length; ++i)
            {
                heightMap.Add(data[i]);
            }
            fs.Close();
            // ReloadMesh();
            List<Material> materials = new List<Material>();
            GetComponent<MeshRenderer>().GetMaterials(materials);

        }
    }

    public float RidgedPerlin(float x, float z)
    {
        float a = amplitude;
        float f = frequency;
        float h = 0f;

        for (int o = 0; o < 8; ++o)
        {
            h += Mathf.PerlinNoise((x + offsetU) * f, (z + offsetV) * f) * a;
            h = amplitude - h;
            a *= loss;
            f *= lacunarity;
        }

        h = Mathf.Abs(h);
        h *= 1;

        return h;
    }

    public void ReloadMesh()
    {
        if(heightMap.Count == 0)
        {
            Debug.LogWarning("Height map empty. Aborting reload.");
            return;
        }


        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        //Calculate positions.
        vertices = new List<Vector3>();
        for (int x = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z)
            {
                float nx = scale * ((float)x / (float)resolution);
                float nz = scale * ((float)z / (float)resolution);
                vertices.Add(new Vector3(nx, 0f + heightMap[(x * resolution) + z], nz));
            }
        }

        mesh.vertices = vertices.ToArray();

        List<int> indices = new List<int>();
        //Calculate indices for each polygon.

        for (int x = 0; x < resolution - 1; ++x)
        {
            for (int z = 0; z < resolution - 1; ++z)
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

    public void Smooth()
    {
        float sum = 0f;
        float[] heightMapArray = new float[heightMap.ToArray().Length];
        heightMapArray = heightMap.ToArray();
        int index = 0;
        float neighbourCount = 0f;

        heightMap.Clear();

        for (int x = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z)
            {
                sum = 0f;
                neighbourCount = 0f;
                index = (x * resolution) + z;

                if(index + 1 < resolution * resolution)
                {
                    sum += heightMapArray[index + 1];
                    neighbourCount += 1f;
                }

                if(index - 1 > 0)
                {
                    sum += heightMapArray[index - 1];
                    neighbourCount += 1f;
                }

                if(index + resolution < resolution * resolution)
                {
                    sum += heightMapArray[index + resolution];
                    neighbourCount += 1f;
                }

                if (index - resolution > 0)
                {
                    sum += heightMapArray[index - resolution];
                    neighbourCount += 1f;
                }

                if (index + resolution + 1 < resolution * resolution)
                {
                    sum += heightMapArray[index + resolution + 1];
                    neighbourCount += 1f;
                }

                if (index - resolution + 1 > 0)
                {
                    sum += heightMapArray[index - resolution + 1];
                    neighbourCount += 1f;
                }

                if (index + resolution - 1 < resolution * resolution)
                {
                    sum += heightMapArray[index + resolution - 1];
                    neighbourCount += 1f;
                }

                if (index - resolution - 1 > 0)
                {
                    sum += heightMapArray[index - resolution - 1];
                    neighbourCount += 1f;
                }


                if(neighbourCount > 0f)
                {
                    heightMapArray[index] = (sum / neighbourCount);
                    heightMap.Add(heightMapArray[index]);
                }
                else
                {
                    heightMap.Add(heightMapArray[index]);
                }
            }
        }
    }

    public void GenerateMesh()
    {

        DestroyImmediate(meshRenderer);
        DestroyImmediate(meshFilter);
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Custom/CustomLit"));
        meshFilter = gameObject.AddComponent<MeshFilter>();
        

        Mesh mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32;
        //Calculate positions.
        List<Vector3> vertices = new List<Vector3>();

        for (int x = 0; x < resolution; ++x)
        {
            for (int z = 0; z < resolution; ++z)
            {
                float height = RidgedPerlin(x, z);
                float nx = scale * ((float)x / (float)resolution);
                float nz = scale * ((float)z / (float)resolution);
                vertices.Add(new Vector3(nx, 0f + height, nz));
                heightMap.Add(height);
            }
        }

        mesh.vertices = vertices.ToArray();

        List<int> indices = new List<int>();

        //Calculate indices for each triangle.
        for (int x = 0; x < resolution - 1; ++x)
        {
            for (int z = 0; z < resolution - 1; ++z)
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
                normals.Add(new Vector3(0, -1, 0));
            }
        }

        mesh.normals = normals.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
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

    public void Invert()
    {
        for(int x = 0; x < resolution; ++x)
        {
            for(int z = 0; z < resolution; ++z)
            {
                heightMap[(x * resolution) + z] *= -1f;
            }
        }

        ReloadMesh();
    }
   
    /*..Getters and Setters..*/

    public List<float> GetHeightMap() => heightMap;
    public void UpdateHeightMap(List<float> values) => heightMap = values;

    public void SetVertices(List<Vector3> values) => vertices = values;
    public List<Vector3> GetVertices() => vertices;

    public void SetTerrainDimensions(int value) => size = value;
    public void SetResolution(int terrrainDimension) => resolution = terrrainDimension;
    public void SetScale(int value) => scale = value;
    public void SetAmplitude(float amp) => amplitude = amp;
    public void SetFrequency(float freq) => frequency = freq;
    public void SetOffsetU(float oU) => offsetU = oU;
    public void SetOffsetV(float oV) => offsetV = oV;
    public void SetLacunarity(float lac) => lacunarity = lac;
    public void SetPersistence(float los) => loss = los;

    public int GetTerrainDimensions() => size;
    public int GetResolution() => resolution;
    public int GetScale() => scale;
    public float GetAmplitude() => amplitude;
    public float GetFrequency() => frequency;
    public float GetLacunarity() => lacunarity;
    public float GetPersistence() => loss;
    public float GetOffsetU() => offsetU;
    public float GetOffsetV() => offsetV;



    //////////////////////////////////////////////////////
    //HYDRAULIC EROSION CODE
    //////////////////////////////////////////////////////

    // Hydraulic Erosion Attributes
    private int seed = 2;
    private int erosionRadius = 4;
    private float particleIntertia = 0.05f;
    private float sedimentCapacityFactor = 4;
    private float minimumSlope = 0.01f;
    private float erosionFactor = 0.3f;
    private float depositionFactor = 0.3f;
    private float evaporationSpeed = 0.01f;
    private int gravity;
    private int particleLifetime = 90;
    private const int waterVolume = 1;
    private float initialVelocity = 1;

    //Holds the indices of the neighbouring vertices for every vertex on the heightmap.
    int[][] neighbourErosionIndices;

    //Holds the weights applied to neighbouring vertices.
    float[][] neighbourErosionWeights;

    private System.Random randomNumGenerator;

    private int currentSeed;
    private int currentErosionRadius = 0;
    private int currentMapSize = 0;


    private void Initialise(int mapSize, bool reset)
    {
        if (reset || (randomNumGenerator == null))
        {
            randomNumGenerator = new System.Random(seed);
            currentSeed = seed;
        }

        //If the user has made a change to any of the attributes, re-bake weightings on erosion brush.
        if (neighbourErosionIndices == null || currentErosionRadius != erosionRadius || currentMapSize != mapSize)
        {
            InitialiseBrushIndices(mapSize, erosionRadius);
            currentErosionRadius = erosionRadius;
            currentMapSize = mapSize;
        }
    }

    public void Erode(float[] map, int mapSize, int numIterations = 1, bool resetSeed = false)
    {
        Initialise(mapSize, resetSeed);

        for (int iteration = 0; iteration < numIterations; iteration++)
        {

            // Create water particle at random point on map
            float posX = randomNumGenerator.Next(0, mapSize - 1);
            float posY = randomNumGenerator.Next(0, mapSize - 1);
            float dirX = 0;
            float dirY = 0;
            float speed = initialVelocity;
            float water = waterVolume;
            float sediment = 0;

            for (int lifetime = 0; lifetime < particleLifetime; lifetime++)
            {
                //Debug.Log("Current life cycle " + lifetime);

                int nodeX = (int)posX;
                int nodeY = (int)posY;
                int dropletIndex = nodeY * mapSize + nodeX;
                // Calculate particle's offset inside the cell (0,0) = at NW node, (1,1) = at SE node
                float cellOffsetX = posX - nodeX;
                float cellOffsetY = posY - nodeY;

                // Calculate particle's height and direction of flow with bilinear interpolation of surrounding heights
                HeightAndGradient heightAndGradient = CalculateHeightAndGradient(map, mapSize, posX, posY);
                // Update the particle's direction and position (move position 1 unit regardless of speed)
                dirX = (dirX * particleIntertia - heightAndGradient.gradientX * (1 - particleIntertia));
                dirY = (dirY * particleIntertia - heightAndGradient.gradientZ * (1 - particleIntertia));

                // Normalize direction
                float len = Mathf.Sqrt(dirX * dirX + dirY * dirY);
                if (len != 0)
                {
                    dirX /= len;
                    dirY /= len;
                }

                posX += dirX;
                posY += dirY;

                // Stop simulating particle if it's not moving or has flowed over edge of map
                if ((dirX == 0 && dirY == 0) || posX < 0 || posX >= mapSize - 1 || posY < 0 || posY >= mapSize - 1)
                {
                    //Debug.Log("Out of bounds.");
                    break;
                }
                // Find the particle's new height and calculate the deltaHeight
                float newHeight = CalculateHeightAndGradient(map, mapSize, posX, posY).height;
                float deltaHeight = newHeight - heightAndGradient.height;
                // Calculate the particle's sediment capacity (higher when moving fast down a slope and contains lots of water)
                float sedimentCapacity = Mathf.Max(-deltaHeight * speed * water * sedimentCapacityFactor, minimumSlope);
                // If carrying more sediment than capacity, or if flowing uphill:
                if (sediment > sedimentCapacity || deltaHeight > 0)
                {

                    // If moving uphill (deltaHeight > 0) try fill up to the current height, otherwise deposit a fraction of the excess sediment
                    float amountToDeposit = (deltaHeight > 0) ? Mathf.Min(deltaHeight, sediment) : (sediment - sedimentCapacity) * depositionFactor;
                    sediment -= amountToDeposit;

                    // Add the sediment to the four nodes of the current cell using bilinear interpolation
                    // Deposition is not distributed over a radius (like erosion) so that it can fill small pits
                    map[dropletIndex] += amountToDeposit * (1 - cellOffsetX) * (1 - cellOffsetY);
                    map[dropletIndex + 1] += amountToDeposit * cellOffsetX * (1 - cellOffsetY);
                    map[dropletIndex + mapSize] += amountToDeposit * (1 - cellOffsetX) * cellOffsetY;
                    map[dropletIndex + mapSize + 1] += amountToDeposit * cellOffsetX * cellOffsetY;

                }
                else
                {
                    // Erode a fraction of the particle's current carry capacity.
                    // Clamp the erosion to the change in height so that it doesn't dig a hole in the terrain behind the particle
                    float amountToErode = Mathf.Min((sedimentCapacity - sediment) * erosionFactor, -deltaHeight);
                    // Use erosion brush to erode from all nodes inside the particle's erosion radius
                    for (int brushPointIndex = 0; brushPointIndex < neighbourErosionIndices[dropletIndex].Length; brushPointIndex++)
                    {
                        int nodeIndex = neighbourErosionIndices[dropletIndex][brushPointIndex];
                        float weighedErodeAmount = amountToErode * neighbourErosionWeights[dropletIndex][brushPointIndex];
                        float deltaSediment = (map[nodeIndex] < weighedErodeAmount) ? map[nodeIndex] : weighedErodeAmount;

                        map[nodeIndex] -= deltaSediment;
                        sediment += deltaSediment;
                    }
                }

                // Update particle's speed and water content
                speed = Mathf.Sqrt(speed * speed + deltaHeight * gravity);
                water *= (1 - evaporationSpeed);
            }
        }

        heightMap.Clear();
        for (int i = 0; i < map.Length; ++i)
        {
            heightMap.Add(map[i]);
        }

    }

    HeightAndGradient CalculateHeightAndGradient(float[] nodes, int mapSize, float posX, float posZ)
    {
        int coordX = (int)posX;
        int coordZ = (int)posZ;

        // A cell can be defined as a unit sqaure made from the current vertex, the vertex in front, 
        // the vertex to the right and the vertex to the diagonal, right + 1.
        // Calculate the particle's offset inside the cell at (0,0) and (1,1).
        float x = posX - coordX;
        float z = posZ - coordZ;

        //Calculate the heights of each node.
        int nodeIndex = (coordZ * mapSize) + coordX;

        if (nodeIndex > heightMap.Count)
        {
            Debug.LogError("Index out of bounds." + nodeIndex + " Height map size " + heightMap.Count);
        }

        float node00 = nodes[nodeIndex];
        float node01 = nodes[nodeIndex + 1];
        float node10 = nodes[nodeIndex + mapSize];
        float node11 = nodes[nodeIndex + mapSize + 1];

        //Calculate the new direction of the particle.
        float gradientX = (node01 - node00) * (1.0f - z) + (node11 - node10) * z;
        float gradientZ = (node10 - node00) * (1.0f - x) + (node11 - node01) * x;

        //Calculate current height using bilinear interpolation.
        float height = node00 * (1.0f - x) * (1.0f - z) + node10 * x * (1.0f - z) + node10 * (1.0f - x) * z + node01 * x * z;

        return new HeightAndGradient() { height = height, gradientX = gradientX, gradientZ = gradientZ };
    }

    public void InitialiseBrushIndices(int mapSize, int radius)
    {
        neighbourErosionIndices = new int[mapSize * mapSize][];
        neighbourErosionWeights = new float[mapSize * mapSize][];

        int[] xOffsets = new int[radius * radius * 4];
        int[] zOffsets = new int[radius * radius * 4];
        float[] weights = new float[radius * radius * 4];
        float weightSum = 0.0f;
        int addIndex = 0;

        for (int i = 0; i < (mapSize * mapSize); ++i)
        {
            int centerX = i % mapSize;
            int centerZ = i / mapSize;

            if (centerZ <= radius
                || centerZ >= mapSize - radius
                || centerX <= radius + 1
                || centerX >= mapSize - radius)
            {
                weightSum = 0.0f;
                addIndex = 0;

                for (int z = -radius; z <= radius; ++z)
                {
                    for (int x = -radius; x <= radius; ++x)
                    {
                        float sqrDistance = (x * x) + (z * z);
                        if (sqrDistance < (radius * radius))
                        {
                            int coordX = centerX + x;
                            int coordZ = centerZ + z;

                            if ((coordX >= 0) && (coordX < mapSize) && (coordZ >= 0) && (coordZ < mapSize))
                            {
                                float weight = 1 - Mathf.Sqrt(sqrDistance) / radius;
                                weightSum += weight;
                                weights[addIndex] = weight;
                                xOffsets[addIndex] = x;
                                zOffsets[addIndex] = z;
                                addIndex++;
                            }
                        }
                    }
                }
            }

            int numEntries = addIndex;
            neighbourErosionIndices[i] = new int[numEntries];
            neighbourErosionWeights[i] = new float[numEntries];

            for (int j = 0; j < numEntries; ++j)
            {
                neighbourErosionIndices[i][j] = (zOffsets[j] + centerZ) * mapSize + xOffsets[j] + centerX;
                neighbourErosionWeights[i][j] = weights[j] / weightSum;
            }
        }
    }

    struct HeightAndGradient
    {
        public float height;
        public float gradientX;
        public float gradientZ;
    }

    public int GetSeed() => seed;
    public int GetErosionRadius() => erosionRadius;
    public float GetParticleIntertia() => particleIntertia;
    public float GetSedimentCapacityFactor() => sedimentCapacityFactor;
    public float GetMinimumSlope() => minimumSlope;
    public float GetErosionFactor() => erosionFactor;
    public float GetDepositionFactor() => depositionFactor;
    public float GetEvaporationSpeed() => evaporationSpeed;
    public int GetGravity() => gravity;
    public int GetParicleLifetime() => particleLifetime;

    public void SetSeed(int value) => seed = value;
    public void SetErosionRadius(int value) => erosionRadius = value;
    public void SetParticleIntertia(float value) => particleIntertia = value;
    public void SetSedimentCapacityFactor(float value) => sedimentCapacityFactor = value;
    public void SetMinimumSlope(float value) => minimumSlope = value;
    public void SetErosionFactor(float value) => erosionFactor = value;
    public void SetDepositionFactor(float value) => depositionFactor = value;
    public void SetEvaporationSpeed(float value) => evaporationSpeed = value;
    public void SetGravity(int value) => gravity = value;
    public void SetParicleLifetime(int value) => particleLifetime = value;

}
