using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{

    [SerializeField]
    private Transform meshCutterTransform;
    [SerializeField]
    private bool flipTransformsUp;
    [SerializeField]
    private Material thisMaterial;

    // @brief Flips the normal of the mesh cutter object.
    private int transformFlipMultiplier;

    void Start()
    {
        //Get a reference to this objects material.
        List<Material> materials = new List<Material>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.GetMaterials(materials);
        thisMaterial = materials[0];

        transformFlipMultiplier = 1;
        if (flipTransformsUp)
        {
            transformFlipMultiplier = -1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Upload mesh cutter properties to the GPU.
        Vector3 planeNormal = transform.worldToLocalMatrix.MultiplyVector(transformFlipMultiplier * meshCutterTransform.up);
        Vector3 planePosition = meshCutterTransform.position;
        thisMaterial.SetVector("sliceNormal", planeNormal);
        thisMaterial.SetVector("sliceCentre", planePosition);
    }
}
