using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    //Input status
    [SerializeField]
    private bool pressedMouseB0;
    [SerializeField]
    private bool isInteractable;

    //Resource wallet attributes
    [SerializeField]
    private Text resourceText;
    [SerializeField]
    private int resourceWallet;
    [SerializeField]
    private int resourceAmount = 0;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    private Text resourcePickUpText;
    [SerializeField]
    private Image promptImage;

    [SerializeField]
    private Text promptText;

    [SerializeField]
    private GameObject interactable;
    [SerializeField]
    private GameObject resourcePickUpAmountObject;


    //Mining attributes
    [SerializeField]
    private Image miningBar;
    [SerializeField]
    private ParticleSystem miningSparks;
    [SerializeField]
    private float miningDuration = 0f;
    [SerializeField]
    private float miningTimer = 0f;
    [SerializeField]
    private bool hasMined = false;

    private Vector3 oldPos;

    private Transform target;

    private string otherTag = " ";

    private const string buildingTag = "Building";
    private const string resourceTag = "Resource";
    private const string defenceTag = "DefenceTower";
    private const string friendlyTag = "Friendly";
    private const string endLevelTag = "EndLevel";

    // Start is called before the first frame update
    void Start()
    {
        pressedMouseB0 = false;
        isInteractable = false;
        resourceWallet = 0;

        //Fetch a reference to the text components on start().
        resourceText = GameObject.FindGameObjectWithTag("ResourceWallet").GetComponent<Text>();
        resourcePickUpAmountObject = GameObject.FindGameObjectWithTag("ResourcePickUpAmount");
        rectTransform = resourcePickUpAmountObject.GetComponent<RectTransform>();
        resourcePickUpText = resourcePickUpAmountObject.GetComponent<Text>();
        resourcePickUpText.enabled = false;
        promptImage.enabled = false;
        promptText.enabled = false;
        miningBar.enabled = true;
        miningSparks.Stop();
        target = null;
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
                //At this point we have successfully spawned a building.
                //Force object ref null and input false.
                interactable = null;
                isInteractable = false;
                pressedMouseB0 = false;
            }
            else if (interactable.tag == resourceTag)
            {
                HandleResourcePickUp();
            }
          
        }
        else
        {
            ResetMiningProgress();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //NOT USING SPLINE CODE
        //if(collision.gameObject.tag == "Environment")
        //{
        //    Debug.Log("Collision!");
        //    Vector3 direction = (collision.transform.position - characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()) + characterRef.SplineOffset()).normalized;
        //    characterRef.ToggleInput(false);
        //    oldPos = characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()) + characterRef.SplineOffset() - direction;
        //    transform.position = oldPos;
        //
        //    Vector3 newSplineOffset = new Vector3();
        //    Vector2 splinePos = new Vector2(characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()).x,
        //        characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()).z);
        //    Vector2 sign = (splinePos - new Vector2(transform.position.x, transform.position.z)).normalized;
        //    newSplineOffset = characterRef.SplineOffset() + new Vector3(sign.x, 0f, sign.y);
        //    characterRef.UpdateOffset(newSplineOffset);
        //}
    }

    private IEnumerator RepositionCharacter(Vector3 oldPos)
    {
/*
        // Debug.Log("Reposition Character!");
        //Vector3 position = characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()) + characterRef.SplineOffset() - direction;
        while (Vector3.Distance(transform.position, oldPos) > 0.5f)
        {
            transform.position = Vector3.Lerp(characterRef.GetSpline().GetPointOnSpline(characterRef.DistanceAlongSpline()) + characterRef.SplineOffset(), oldPos, 10f * Time.deltaTime);
            Debug.Log("Distance to completion " + Vector3.Distance(transform.position, oldPos));
            yield return null;
        }

        characterRef.ToggleInput(true);
*/
        yield return null;
    }

    public void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.tag == "Environment")
        //{
        //   // characterRef.ToggleInput(true);
        //}
    }

    public void OnTriggerStay(Collider other)
    {
        otherTag = other.gameObject.tag;

        //Is the object a resource or building.
        if (otherTag == resourceTag || otherTag == buildingTag || otherTag == defenceTag || otherTag == friendlyTag)
        {
            if(target == null && (otherTag != resourceTag))
            {
                for (int i = 0; i < other.transform.childCount; ++i)
                {
                    if (other.transform.GetChild(i).tag == "InfoPanel")
                    {
                        target = other.transform.GetChild(i).transform;
                    }
                }
            }
            else
            {
                target = null;
            }
            interactable = other.gameObject;
            isInteractable = true;
            promptImage.enabled = true;
            promptText.enabled = true;
        }

        if (otherTag == endLevelTag)
        {
            other.gameObject.GetComponent<EndLevelScript>().ChangeLevel();
        }

        if (otherTag == friendlyTag)
        {
            interactable = other.gameObject;
            interactable.GetComponent<FriendlyController>().SetRecruited(true);
            promptImage.enabled = false;
            promptText.enabled = false;
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
            promptImage.enabled = false;
            promptText.enabled = false;
            target = null;
        }
    }

    // @brief Handle the picking up of resources.
    private void HandleResourcePickUp()
    {
        MiningAnimation();
        //Animate mining bar.
        if (hasMined)
        {
            StartCoroutine("AnimatePlayerUI");
            Destroy(interactable);
            //At this point we have successfully mined.
            //Force object ref null and input false.
            interactable = null;
            isInteractable = false;
            pressedMouseB0 = false;
            promptImage.enabled = false;
            promptText.enabled = false;
            miningBar.enabled = false;
        }
    }

    private void MiningAnimation()
    {
        RectTransform rect = miningBar.rectTransform;

        if (miningTimer < miningDuration)
        {
            //Play the emitter.
            if(!miningSparks.isPlaying)
                miningSparks.Play();

            miningBar.enabled = true;
            promptText.enabled = false;
            

            //Incriment timer.
            miningTimer += Time.deltaTime;
            float percentage = miningTimer / miningDuration;
            float width = rect.rect.width;
            float w = MathsUtils.RemapRange(percentage, 0, 1, 0, 180);
            width = w;
            rect.sizeDelta = new Vector2(width, 50);
        }   
        else
        {
            hasMined = true;
        }
    }

    private void ResetMiningProgress()
    {
       
        //Reset things
        miningTimer = 0f;

        //Flag we're finished mining.
        hasMined = false;

        //Stop emitter.
        miningSparks.Stop();

        //Reset mining bar progress.
        miningBar.rectTransform.sizeDelta = new Vector2(0, 50);

        //Hide the promt UI.
        miningBar.enabled = false;

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

        ResetMiningProgress();


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

    public bool IsInteracting()
    {
        return interactable;
    }

    public Transform GetTarget() => target;
    
}
