using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicErosion : MonoBehaviour
{

    [SerializeField]
    private int seed = 2;
    [SerializeField]
    private int erosionRadius;
    [SerializeField]
    private float particleIntertia = 0.05f;
    [SerializeField]
    private float sedimentCapacityFactor = 4;
    [SerializeField]
    private float minimumSlope = 0.01f;
    [SerializeField]
    private float erosionFactor = 0.3f;
    [SerializeField]
    private float depositionFactor = 0.3f;
    [SerializeField]
    private float evaporationSpeed = 0.01f;
    [SerializeField]
    private int gravity;
    [SerializeField]
    private int particleLifetime = 20;
    [SerializeField]
    private const int waterVolume = 1;
    [SerializeField]
    private float initialVelocity = 1;

    //Holds the indices of the neighbouring vertices for every vertex on the heightmap.
    int[][] neighbourErosionIndices;

    //Holds the weights applied to neighbouring vertices.
    float[][] neighbourErosionWeights;

    private System.Random randomNumGenerator;

    private int currentSeed;
    private int currentErosionRadius;
    private int currentMapSize;

    public void Initialise(int mapSize, bool reset)
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

        for (int i = 0; i < neighbourErosionIndices.GetLength(0); ++i)
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
