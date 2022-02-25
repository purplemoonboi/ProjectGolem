using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{

    [SerializeField]
    private bool pressedMouseB0;
    [SerializeField]
    private bool isInteractable;
    [SerializeField]
    private Canvas canvas;

    private Text resourceText;

    [SerializeField]
    private int resourceWallet;
    [SerializeField]
    private int resourceAmount = 150;

    [SerializeField]
    private GameObject interactable;

    private string otherTag = " ";

    private const string buildingTag = "Building";
    private const string resourceTag = "Resource";

    // Start is called before the first frame update
    void Start()
    {
        pressedMouseB0 = false;
        isInteractable = false;
        resourceWallet = 0;
        resourceText = canvas.GetComponentsInChildren<Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pressedMouseB0 = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressedMouseB0 = false;
        }

        if (interactable != null && isInteractable)
        {
            ProcessInteractions();
        }
    }

    private void ProcessInteractions()
    {
        //Handle resources
        if(interactable != null && pressedMouseB0)
        {
            if (interactable.tag == buildingTag)
            {
                HandleBuilding();
            }
            else if (interactable.tag == resourceTag)
            {
                HandleResourcePickUp();
            }

            //At this point we have successfully spawned a building.
            //Force object ref null and input false.
            interactable = null;
            isInteractable = false;
        }
          
    }

    public void OnTriggerEnter(Collider other)
    {
        otherTag = other.gameObject.tag;

        //Is the object a resource or building.
        if (otherTag == resourceTag || otherTag == buildingTag)
        {
            interactable = other.gameObject;
            isInteractable = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == resourceTag || other.gameObject.tag == buildingTag)
        {
            //Void data.
            interactable = null;
            isInteractable = false;
        }
    }

    // @brief Handle the picking up of resources.
    private void HandleResourcePickUp()
    {
        Debug.Log("Picked up resource!");
        resourceWallet += resourceAmount;
        resourceText.text = resourceWallet.ToString();
        Destroy(interactable);
        interactable = null;
        pressedMouseB0 = false;
        isInteractable = false;
    }


    private void HandleBuilding()
    {
        Building building = interactable.GetComponent<Building>();

        if(building == null)
        {
            Debug.LogError("Building is a null reference!");
        }
        else
        {
            //If not currently active spawn building procedure.
            if (!building.IsActive())
            {
                if (building.GetCost() <= resourceWallet)
                {
                    resourceWallet -= building.GetCost();
                    //Update UI.
                    resourceText.text = resourceWallet.ToString();
                    //Tell the building to begin spawn animation.
                    building.SetShouldSpawn(true);
                }
            }
            else if (building.IsActive() && !building.IsSpawning() && !building.IsMaxLevel())
            {
                //Upgrade building.
                if (building.GetCostToUpgrade() <= resourceWallet)
                {
                    resourceWallet -= building.GetCost();
                    //Update UI.
                    resourceText.text = resourceWallet.ToString();
                    switch (building.GetBuildingType())
                    {
                        case BuildingType.Builder:
                            BuilderBuilding builderBuilding     = (BuilderBuilding)building;
                            builderBuilding.Upgrade();
                            break;
                        case BuildingType.Weapons:
                            WeaponsBuilding weaponsBuilding     = (WeaponsBuilding)building;
                            weaponsBuilding.Upgrade();
                            break;
                        case BuildingType.Barricade:
                            BarricadeBuilding barricadeBuilding = (BarricadeBuilding)building;
                            barricadeBuilding.Upgrade();
                            break;
                        case BuildingType.Turret:
                            TurretBuilding turretBuilding       = (TurretBuilding)building;
                            turretBuilding.Upgrade();
                            break;
                        case BuildingType.Camp:
                            CampBuilding campBuilding           = (CampBuilding)building;
                            campBuilding.Upgrade();
                            break;
                    }
                }
            }
        }
    }

    private void RenderInfoPanel()
    {
        if(interactable.tag == resourceTag || interactable.tag == buildingTag)
        {
            Canvas infoPanel = CreateCanvas();
        }
    }

    private Canvas CreateCanvas()
    {
        Canvas canvas = new Canvas();
        

        return canvas;
    }

    public int GetResources()
    {
        return resourceWallet;
    }

    public void UpdateResources(int value)
    {
        resourceWallet += value;
    }
}
