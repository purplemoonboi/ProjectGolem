using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsBuilding : Building
{

    [SerializeField]
    private WeaponsScriptableObject weaponsScriptableObject;

    [SerializeField]
    private FriendlyScriptableObject weaponStatistics;
    [SerializeField]
    private float damageIncrease = 5f;


    public void SetLevel(int value) => buildingInfo.level = value;

    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Weapons);
        ResetParameters();
        this.buildingInfoCanvas.SetActive(false);

        weaponsScriptableObject.isMaxLevel = false;
        weaponsScriptableObject.level = 1;
        weaponStatistics.power = 15f;
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
        weaponStatistics.power += damageIncrease;
        buildingInfo.level = GetLevel();
    }

    /*..Trigger callback methods..*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
            // buildingInfo.EnableInfoPanel();
            //
            // string[] infoArray =
            // {
            //      "Health" + health.ToString(),
            //      "Level " + GetLevel().ToString(),
            //      (!isActive) ? "Cost to build " + GetCost().ToString() : "Cost to upgrade " + GetCostToUpgrade().ToString(),
            //      buildingType.ToString()
            // };
            //
            //
            // Text infoText = GetComponentInChildren<Text>();
            // infoText.text = " ";
            //
            // for (int i = 0; i < infoArray.Length; ++i)
            // {
            //     infoText.text = infoText.text + "\n" + infoArray[i];
            // }
            //
            // buildingInfo.SetText(infoText);

            if (other.tag == "Player")
            {
                if (!other.GetComponent<Interact>().IsTalking())
                {
                    this.buildingInfoCanvas.SetActive(true);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
            //buildingInfo.DisableInfoPanel();
            //Text infoText = GetComponentInChildren<Text>();
            //infoText.text = " ";
            this.buildingInfoCanvas.SetActive(false);
        }
    }

    public FriendlyScriptableObject GetWeaponStats() => weaponStatistics;

    protected void IncrimentBuildingLevel() => weaponsScriptableObject.level++;

    public void SetMaxHealth(int _health) => weaponsScriptableObject.maximumHealth += _health;

    public bool IsMaxLevel() => weaponsScriptableObject.isMaxLevel;

    public int GetLevel() => weaponsScriptableObject.level;

    public int GetMaxLevel() => weaponsScriptableObject.maxLevel;
}
