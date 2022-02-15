using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{

    [SerializeField]
    private Transform meshCutterTransform;
    [SerializeField]
    private bool flipTransformsUp = true;
    [SerializeField]
    public List<Material> hologramMaterials;

    // @brief Flips the normal of the mesh cutter object.
    private int transformFlipMultiplier = 1;

    void Start()
    {
        hologramMaterials = new List<Material>();

        //We need to grab childrens materials if they are tagged hologram.
        Renderer[] renderersInChildren = GetComponentsInChildren<Renderer>();

        //Get all the materials in the child game objects.
        foreach(Renderer renderer in renderersInChildren)
        {
            List<Material> materials = new List<Material>();
            renderer.GetMaterials(materials);

            if(materials != null)
            {
                foreach(Material material in materials)
                {
                    Debug.Log("Added material reference!");
                    hologramMaterials.Add(material);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 planeNormal = transform.worldToLocalMatrix.MultiplyVector(transformFlipMultiplier * meshCutterTransform.up);
        Vector3 planePosition = meshCutterTransform.localPosition;

        // Upload mesh cutter properties to the GPU.
        foreach (Material material in hologramMaterials)
        {
            material.SetVector("sliceNormal", planeNormal);
            material.SetVector("sliceCentre", planePosition);
        }
    }
}
