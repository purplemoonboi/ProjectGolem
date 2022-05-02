using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampBuilding : Building
{

    [SerializeField]
    private CampScriptableObject campScriptableObject;
    [SerializeField]
    private BuilderScriptableObject builderScriptableObject;
    [SerializeField]
    private WeaponsScriptableObject weaponsScriptableObject;
    [SerializeField]
    private FriendlyScriptableObject weaponStatistics;
    [SerializeField]
    private float bonusStat = 5f;



    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Camp);
        ResetParameters();
        campScriptableObject.isMaxLevel = false;
        campScriptableObject.level = 1;
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
        SetMaxHealth((int)GetHealth() + 100);

        builderScriptableObject.maximumHealth += bonusStat;
        weaponsScriptableObject.maximumHealth += bonusStat;
        weaponStatistics.power += bonusStat;
        weaponStatistics.repairRate += bonusStat;
    }

    /*..Trigger callback methods..*/

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
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
        }
    }

    protected void IncrimentBuildingLevel() => campScriptableObject.level++;

    public void SetMaxHealth(int _health) => campScriptableObject.maximumHealth += _health;

    public bool IsMaxLevel() => campScriptableObject.isMaxLevel;

    public int GetLevel() => campScriptableObject.level;

    public int GetMaxLevel() => campScriptableObject.maxLevel;
}
