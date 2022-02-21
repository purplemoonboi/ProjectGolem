using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(TerrainScript))]
public class LevelGenerator : Editor
{
    [SerializeField]
    [Tooltip("Octaves are the number of layers of noise you apply to the terrain.")]
    private int octave = 8;
    [SerializeField]
    [Tooltip("The amount that frequecny will increase by per octave.")]
    private float lacunarity = 2.0f;
    [SerializeField]
    [Tooltip("Gain is actually the amount amplitude with drop by per octave.")]
    private float gain = 0.4f;
    [SerializeField]
    private float width;
    [SerializeField]
    private float breadth;
    [SerializeField]
    private float offsetX = 1.0f;
    [SerializeField]
    private float offsetZ = 1.0f;


    public override void OnInspectorGUI()
    {
        //The target object.
        TerrainScript thisTarget = (TerrainScript)target;

        //GUI controls to change the terrains parameters.
        width = EditorGUILayout.Slider("Width", thisTarget.GetWidth(), 2, 1024);
        breadth = EditorGUILayout.Slider("Breadth", thisTarget.GetBreadth(), 2, 1024);

        octave = EditorGUILayout.IntSlider("Octave", octave, 1, 16);
        lacunarity = EditorGUILayout.Slider("Lacunarity", lacunarity, 1.1f, 10.0f);
        gain = EditorGUILayout.Slider("Gain", gain, 0.01f, 0.9f);

        float offX = EditorGUILayout.Slider("Offset X", offsetX, 1.0f, 10000.0f);
        float offZ = EditorGUILayout.Slider("Offset Z", offsetZ, 1.0f, 10000.0f);

        //Update the terrains width and height.
        thisTarget.SetWidthAndBreadth(width, breadth);

        thisTarget.SetOctaves(octave);

        //Apply the noise algorithm.
        if(GUILayout.Button("Apply noise"))
        {
            thisTarget.Noise(lacunarity, gain, offsetX, offsetZ);
        }

    }



}
