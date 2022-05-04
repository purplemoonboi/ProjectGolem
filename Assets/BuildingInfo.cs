using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfo : MonoBehaviour
{
    [SerializeField]
    private Building building;
    [SerializeField]
    private Text[] textFields;

    public int level;

    // Start is called before the first frame update
    void Start()
    {
        textFields = new Text[6];
        textFields = GetComponentsInChildren<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (building != null)
        {
            Debug.Log("Doing stuff");
            textFields[0].text = "Building " + building.GetBuildingType().ToString();
            textFields[1].text = "Level " + level.ToString();
            textFields[2].text = "Health " + building.GetHealth().ToString();
            if (building.GetBuildingType() == BuildingType.Weapons)
            {
                //Turret and friendly DPS output
                WeaponsBuilding weaponsBuilding = (WeaponsBuilding)building;
                textFields[3].text = "Turret Damage Output " + weaponsBuilding.GetWeaponStats().power.ToString();
                textFields[4].text = "Friendly Damage Output " + weaponsBuilding.GetWeaponStats().power.ToString();
            }
            else if (building.GetBuildingType() == BuildingType.Builder)
            {
                BuilderBuilding builderBuilding = (BuilderBuilding)building;
                textFields[3].text = "Repair Rate ";
            }

            if (!building.IsActive())
            {
                textFields[5].text = "Cost to buy " + building.GetCost().ToString();
            }
            else
            {
                textFields[5].text = "Cost to upgrade " + building.GetCostToUpgrade().ToString();
            }
        }
    }
}
