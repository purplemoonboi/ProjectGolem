using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBuilding : Building
{

    // Start is called before the first frame update
    void Start()
    {
        ResetParameters();
        SetCostToUpgrade(85);
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

        Debug.Log("Upgraded builder building, level upgraded to " + GetLevel());
    }


}
