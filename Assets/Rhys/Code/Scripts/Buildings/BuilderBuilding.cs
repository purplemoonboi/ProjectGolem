using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderBuilding : Building
{

    [SerializeField]
    private BuilderScriptableObject builderScriptableObject;
    [SerializeField]
    private BuildingInfoPanel buildingInfo;

    bool updateUI = false;

    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Builder);
        ResetParameters();
        builderScriptableObject.isMaxLevel = false;
        builderScriptableObject.level = 1;
        buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn())
        {
            Debug.Log("Now spawning building.");
            StartCoroutine("PlaySpawnAnimation");
        }
    }

    public override void Upgrade()
    {
        IncrimentBuildingLevel();
        SetCostToUpgrade(GetCostToUpgrade());
    }

    /*..Trigger callback methods..*/

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            buildingInfo.EnableInfoPanel();

            string[] infoArray =
            {
                 health.ToString(),
                 "Level " + GetLevel().ToString(),
                 (!isActive) ? "Cost to build " + GetCost().ToString() : "Cost to upgrade " + GetCostToUpgrade().ToString(),
                 buildingType.ToString()
            };


            Text infoText = GetComponentInChildren<Text>();
            infoText.text = " ";
            for (int i = 0; i < infoArray.Length; ++i)
            {
                infoText.text = infoText.text + "\n" + infoArray[i];
            }

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
            updateUI = false;
        }
    }

    protected void IncrimentBuildingLevel() => builderScriptableObject.level++;

    public void SetMaxHealth(int _health) => builderScriptableObject.maximumHealth += _health;

    public bool IsMaxLevel() => builderScriptableObject.isMaxLevel;

    public int GetLevel() => builderScriptableObject.level;

    public int GetMaxLevel() => builderScriptableObject.maxLevel;
}
