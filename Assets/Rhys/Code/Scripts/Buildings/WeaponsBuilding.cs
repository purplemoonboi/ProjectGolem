using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsBuilding : Building
{
    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Weapons);
        ResetParameters();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpawning())
        {
            Debug.Log("Now spawning building.");
            PlaySpawnAnimation();
        }
    }


    public override void Upgrade()
    {
        IncrimentBuildingLevel();
        SetCostToUpgrade(GetCostToUpgrade() + 50);
    }
}
