using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    [SerializeField]
    private GridManager gridManager;
    [SerializeField]
    RaycastHit lastRayHitData;
    [SerializeField]
    RaycastHit currentHitData;
    [SerializeField]
    private bool castRay;
    [SerializeField]
    private int gridMask = (1 << 8);
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float rayDistance;

    // Start is called before the first frame update
    void Start()
    {
        castRay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            castRay = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            castRay = false;
        }
    }

    void FixedUpdate()
    {
        if(castRay)
        {
            CastRay();
        }
    }

   // void CastRay()
   // {
   //     int gridMask = (1 << 8);
   //     RaycastHit hit;
   //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
   //     Debug.Log("Ray : " + ray);
   //
   //     if (Physics.Raycast(ray, out hit, 1000.0f, gridMask))
   //     {
   //         Debug.Log("Cell position " + hit.transform.position);
   //
   //         //Reference to the cell.
   //         GameObject gameObject = hit.transform.gameObject;
   //
   //         List<Material> materials = new List<Material>();
   //         MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
   //         meshRenderer.GetMaterials(materials);
   //
   //         materials[0].SetColor("_Color", Color.green);
   //     }
   // }

    // @brief Casts a ray from the camera towards the direction it is facing.
    void CastRay()
    {
        //Define a ray from the camrea's position towards the direction it is facing.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Fire");
        if (Physics.Raycast(ray, out currentHitData, rayDistance, gridMask))
        {
            //We hit a cell, is it occupied.
            Debug.Log("Indices x : " + (int)currentHitData.transform.position.x);
            Debug.Log("Indices z : " + (int)currentHitData.transform.position.z);
            Vector2Int gridIndices = new Vector2Int((int)currentHitData.transform.position.x, (int)currentHitData.transform.position.z);

            Debug.Log("The current cell is occupied? : " + gridManager.IsOccupied(gridIndices.x, gridIndices.y));

            //Record what we previously hit.
            lastRayHitData = currentHitData;
        }

    }

}   
