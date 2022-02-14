using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnBuilding : MonoBehaviour
{

    private bool isInteracting;

    private AnimateBuildingSpawn animateBuildingSpawn;


    // Start is called before the first frame update
    void Start()
    {
        isInteracting = false;
        animateBuildingSpawn = null;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isInteracting = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
        }


    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Building" && isInteracting)
        {
            //If null try get the component.
            if(animateBuildingSpawn == null)
            {
                animateBuildingSpawn = other.GetComponentInChildren<AnimateBuildingSpawn>();
                if(animateBuildingSpawn.HasAlreadySpawnedBuilding() == false)
                {
                    animateBuildingSpawn.SetShouldMovePlane(true);
                    Debug.Log("Instructed the building animation.");
                }
            }
        }
    }

   
}
