using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    private BuildingScriptableObject baseBuildingData;
    [SerializeField]
    private Transform meshCutterTransform;
    [SerializeField]
    private Transform spawnerEndGoal;
    [SerializeField]
    private int cost;
    [SerializeField]
    private int costToUpgrade;


    //Moves the plane towards the goal thus rendering the building as it
    //passes through the hologram.
    protected void PlaySpawnAnimation()
    {
        Debug.Log("Animating buildings.");

        Vector3 currentPosition = meshCutterTransform.position;
        Vector3 direction = Vector3.Normalize(spawnerEndGoal.position - currentPosition);

        currentPosition += direction * 10.0f * Time.deltaTime;

        if (Vector3.Distance(currentPosition, spawnerEndGoal.position) < 0.001f)
        {
            baseBuildingData.isActive   = true;
            baseBuildingData.isSpawning = false;
        }

        meshCutterTransform.position = currentPosition;
    }

    // @brief All buildings contain upgrade method.
    public abstract void Upgrade();

    public void SetShouldSpawn(bool value)
    {
        baseBuildingData.isSpawning = value;
    }

    protected void IncrimentBuildingLevel()
    {
        if (baseBuildingData.level < baseBuildingData.maxLevel)
        {
            baseBuildingData.level++;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
            buildingInfo.EnableInfoPanel();

            string[] infoArray =
            {
                 baseBuildingData.prefabName.ToString(),
                 "Level " + GetLevel().ToString(),
                 (!baseBuildingData.isActive) ? "Cost to build " + GetCost().ToString() : "Cost to upgrade " + GetCostToUpgrade().ToString(),
                 baseBuildingData.buildingType.ToString()
            };

            Text infoText = GetComponentInChildren<Text>();
            for(int i = 0; i < infoArray.Length; ++i)
            {
                infoText.text = infoText.text + "\n" + infoArray[i];
            }

            Debug.Log("Building Info" + infoText.text);

            buildingInfo.SetText(infoText);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
            buildingInfo.DisableInfoPanel();
            Text infoText = GetComponentInChildren<Text>();
            infoText.text = " ";
        }
    }

    /*..Public Setters..*/

    public void SetCost(int _cost) => baseBuildingData.cost = _cost;

    public void SetCostToUpgrade(int _costToUpgrade) => baseBuildingData.costToUpgrade = _costToUpgrade;

    public void SetMaxHealth(int _health) => baseBuildingData.maximumHealth += _health;

    public void SetBuildingType(BuildingType type) => baseBuildingData.buildingType = type;

    public void ResetParameters()
    {
        baseBuildingData.isActive = false;
        baseBuildingData.isSpawning = false;
        baseBuildingData.isMaxLevel = false;
        baseBuildingData.level = 1;
        baseBuildingData.costToUpgrade = baseBuildingData.initCostToUpgrade;
    }

    public void BuildingCosts(int _cost, int _costToUpgrade)
    {
        cost = _cost;
        costToUpgrade = _costToUpgrade;
    }

    /*..Public Getters..*/

    public int GetCost()                    => baseBuildingData.cost;
    
    public BuildingType GetBuildingType()   => baseBuildingData.buildingType;
    
    public bool IsActive()                  => baseBuildingData.isActive;

    public int GetCostToUpgrade()           => baseBuildingData.costToUpgrade;

    public bool IsSpawning()                => baseBuildingData.isSpawning;

    public bool IsMaxLevel()                => baseBuildingData.isMaxLevel;

    public int GetLevel()                   => baseBuildingData.level;
}
