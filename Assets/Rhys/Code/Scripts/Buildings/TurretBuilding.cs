using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretBuilding : Building
{

    [SerializeField]
    private TurretScriptableObject turretScriptableObject;
    [SerializeField]
    private TurretStats turretStats;
    [SerializeField]
    private GameObject doubleBarrelHoloGFX;
    [SerializeField]
    private GameObject doubleBarrelGFX;

    public AudioSource rotateAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Turret);
        ResetParameters();
        turretScriptableObject.isMaxLevel = false;
        turretScriptableObject.level = 1;
        
        turretScriptableObject.cost = cost;
        turretScriptableObject.costToUpgrade = costToUpgrade;
        doubleBarrelGFX.SetActive(false);
        turretStats.SetLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldSpawn())
        {
            Debug.Log("Now spawning building.");
            StartCoroutine("PlaySpawnAnimation");
            turretStats.SetIsActivated(true);
        }

        if(turretStats.GetHealth() <= 0.0f)
        {
            //Upon death the turret is no longer active.
            isActive = false;
            shouldSpawn = false;
            turretStats.SetIsActivated(false);
            //Re-enable holographic GFX.
            ToggleHolographicGFX(true);
        }

    }

    public override void Upgrade()
    {
        IncrimentBuildingLevel();
        turretStats.SetLevel(turretScriptableObject.level);
        SetCostToUpgrade(turretScriptableObject.costToUpgrade);
        SetMaxHealth((int)GetHealth() + 100);

        //If level 2 activate the extra barrels.
        if(turretStats.Level == 2)
        {
            doubleBarrelHoloGFX.SetActive(false);
            doubleBarrelGFX.SetActive(true);
        }

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
                 name,
                 health.ToString(),
                 "Level " + GetLevel().ToString(),
                 (!isActive) ? "Cost to build " + turretScriptableObject.cost.ToString() : "Cost to upgrade " + turretScriptableObject.costToUpgrade.ToString(),
                 buildingType.ToString()
            };

            Text infoText = GetComponentInChildren<Text>();
            infoText.text = " ";

            for (int i = 0; i < infoArray.Length; ++i)
            {
                infoText.text = infoText.text + "\n" + infoArray[i];
            }

            buildingInfo.SetText(infoText);

            if(turretStats.Level < 2)
            {
                doubleBarrelHoloGFX.SetActive(true);
            }

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
            if (turretStats.Level < 2)
            {
                doubleBarrelHoloGFX.SetActive(false);
            }
        }
    }

    protected void IncrimentBuildingLevel() => turretScriptableObject.level++;

    public void SetMaxHealth(int _health) => turretScriptableObject.maximumHealth += _health;

    public bool IsMaxLevel() => turretScriptableObject.isMaxLevel;

    public int GetLevel() => turretScriptableObject.level;

    public int GetMaxLevel() => turretScriptableObject.maxLevel;

    /*..Public overrides..*/

    public override int GetCost() => turretScriptableObject.cost;

    public override int GetCostToUpgrade() => turretScriptableObject.costToUpgrade;

    public override void SetCost(int _cost) => turretScriptableObject.cost = _cost;

    public override void SetCostToUpgrade(int _costToUpgrade) => turretScriptableObject.costToUpgrade = _costToUpgrade;


}
