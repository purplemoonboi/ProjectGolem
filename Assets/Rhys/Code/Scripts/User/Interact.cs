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
    private Text resourceText;
    [SerializeField]
    private int resourceWallet;
    [SerializeField]
    private int resourceAmount = 0;
   


    [SerializeField]
    private GameObject interactable;
    [SerializeField]
    private GameObject resourcePickUpAmountObject;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    private Text resourcePickUpText;

    private string otherTag = " ";

    private const string buildingTag = "Building";
    private const string resourceTag = "Resource";
    private const string defenceTag = "DefenceTower";

    // Start is called before the first frame update
    void Start()
    {
        pressedMouseB0 = false;
        isInteractable = false;
        resourceWallet = 25;

        //Fetch a reference to the text components on start().
        resourceText = GameObject.FindGameObjectWithTag("ResourceWallet").GetComponent<Text>();
        resourcePickUpAmountObject = GameObject.FindGameObjectWithTag("ResourcePickUpAmount");
        rectTransform = resourcePickUpAmountObject.GetComponent<RectTransform>();
        resourcePickUpText = resourcePickUpAmountObject.GetComponent<Text>();

        resourcePickUpText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
            if (interactable.tag == buildingTag || interactable.tag == defenceTag)
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
            pressedMouseB0 = false;
        }
          
    }

    public void OnTriggerStay(Collider other)
    {
        otherTag = other.gameObject.tag;

        //Is the object a resource or building.
        if (otherTag == resourceTag || otherTag == buildingTag || otherTag == defenceTag)
        {
            interactable = other.gameObject;
            isInteractable = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        otherTag = other.gameObject.tag;
        if (otherTag == resourceTag || otherTag == buildingTag || otherTag == defenceTag)
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
        StartCoroutine("AnimatePlayerUI");
        Destroy(interactable);
        interactable = null;
        pressedMouseB0 = false;
        isInteractable = false;
    }

    private IEnumerator AnimatePlayerUI()
    {

        //Switch the resource amount pickup on.
        
        Color colour = resourcePickUpText.color;
        Vector2 currentTextPosition = rectTransform.position;
        Vector2 initialPosition = rectTransform.position;
        resourcePickUpText.enabled = true;
        string resourceAmountText = resourceAmount.ToString();

        resourcePickUpText.text = resourceAmountText;


        //Animate the text a little bit.
        while (colour.a > 0)
        {
            colour.a -= 2 * Time.deltaTime;
            Debug.Log("Alpha " + colour.a);
            currentTextPosition.y += 25.0f * Time.deltaTime;
            //Update references.
            resourcePickUpText.color = colour;
            rectTransform.position = currentTextPosition;
            //Return from the function and continue main loop.
            yield return null;
        }


        //Switch it off.
        resourcePickUpText.enabled = false;
        colour.a = 1.0f;
        resourcePickUpText.color = colour;
        rectTransform.position = initialPosition;

        //Animate the text a little bit.
        int currentWallet = resourceWallet;
        float timer = resourceWallet;
        while (resourceWallet != (currentWallet + resourceAmount))
        {
            timer += resourceAmount * Time.deltaTime;
            resourceWallet = (int)timer;
            resourceText.text = resourceWallet.ToString();
            //Return from the function and continue main loop.
            yield return null;
        }

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
                    building.Spawn();
                }
            }
            else if (building.IsActive())
            {
                //Upgrade building.
                if (building.GetCostToUpgrade() <= resourceWallet)
                {
                    switch (building.GetBuildingType())
                    {
                        case BuildingType.Builder:
                            BuilderBuilding builderBuilding = (BuilderBuilding)building;
                            if (!builderBuilding.IsMaxLevel())
                            {
                                resourceWallet -= builderBuilding.GetCostToUpgrade();
                                //Update UI.
                                resourceText.text = resourceWallet.ToString();
                                builderBuilding.Upgrade();
                            }
                            break;
                        case BuildingType.Weapons:
                            WeaponsBuilding weaponsBuilding = (WeaponsBuilding)building;
                            if (!weaponsBuilding.IsMaxLevel())
                            {
                                resourceWallet -= weaponsBuilding.GetCostToUpgrade();
                                //Update UI.
                                resourceText.text = resourceWallet.ToString();
                                weaponsBuilding.Upgrade();
                            }
                            break;
                        case BuildingType.Barricade:
                            BarricadeBuilding barricadeBuilding = (BarricadeBuilding)building;
                            if (!barricadeBuilding.IsMaxLevel())
                            {
                                resourceWallet -= barricadeBuilding.GetCostToUpgrade();
                                //Update UI.
                                resourceText.text = resourceWallet.ToString();
                                barricadeBuilding.Upgrade();
                            }
                            break;
                        case BuildingType.Turret:
                            TurretBuilding turretBuilding = (TurretBuilding)building;
                            if (!turretBuilding.IsMaxLevel())
                            {
                                resourceWallet -= turretBuilding.GetCostToUpgrade();
                                //Update UI.
                                resourceText.text = resourceWallet.ToString();
                                turretBuilding.Upgrade();
                            }
                            break;
                        case BuildingType.Camp:
                            CampBuilding campBuilding = (CampBuilding)building;
                            if (!campBuilding.IsMaxLevel())
                            {
                                resourceWallet -= campBuilding.GetCostToUpgrade();
                                //Update UI.
                                resourceText.text = resourceWallet.ToString();
                                campBuilding.Upgrade();
                            }
                            break;
                    }
                }
            }
        }
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
