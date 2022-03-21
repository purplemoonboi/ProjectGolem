using UnityEditor;

[CustomEditor(typeof(TerrainMesh))]
public class HydraulicErosionInspector : Editor
{

    private TerrainMesh terrainMesh;
    private HydraulicErosion hydraulicErosion;

    private int seed = 2;
    private int erosionRadius;
    private float particleIntertia = 0.05f;
    private float sedimentCapacityFactor = 4;
    private float minimumSlope = 0.01f;
    private float erosionFactor = 0.3f;
    private float depositionFactor = 0.3f;
    private float evaporationSpeed = 0.01f;
    private int gravity;
    private int particleLifetime = 20;
    private const int waterVolume = 1;
    private float initialVelocity = 1;

    // @brief Create tools in the inspector.
    public override void OnInspectorGUI()
    {
        hydraulicErosion = target as HydraulicErosion;

        EditorGUI.BeginChangeCheck();
        seed = terrainMesh.GetResolution();
        seed = EditorGUILayout.IntSlider("Seed", seed, 2, 10);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(hydraulicErosion, "Base Resolution");
            hydraulicErosion.SetSeed(seed);
            EditorUtility.SetDirty(hydraulicErosion);
        }



    }

}
