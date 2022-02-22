using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBuilding : Building
{

    // Start is called before the first frame update
    void Start()
    {
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

        /*
         * 
         *      Upgrades go here.
         *
         */

        SetCostToUpgrade(GetCostToUpgrade() + 50);

        Debug.Log("Upgraded builder building, level upgraded to " + GetLevel());
    }


}
