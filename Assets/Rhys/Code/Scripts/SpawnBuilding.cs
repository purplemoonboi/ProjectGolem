using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnBuilding : MonoBehaviour
{
    [SerializeField]
    private PickupResources pickupResources;
    [SerializeField]
    private Canvas canvas;

    private Text resourceText;

    private bool isInteracting;
    private bool isInBuildingSpawn;
    private AnimateBuildingSpawn animateBuildingSpawn;


    // Start is called before the first frame update
    void Start()
    {
        isInteracting = false;
        animateBuildingSpawn = null;
        isInBuildingSpawn = false;
        resourceText = canvas.GetComponentsInChildren<Text>()[1];
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


        if(isInBuildingSpawn && isInteracting && animateBuildingSpawn != null)
        {
            Debug.Log("Spawning building");
            int currentAmount = pickupResources.GetResources();
            if (currentAmount >= 50)
            {
                pickupResources.UpdateResources(-50);
                currentAmount -= 50;
                resourceText.text = currentAmount.ToString();
                animateBuildingSpawn.SetShouldMovePlane(true);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Building")
        {
            Debug.Log("Is in Building Spawn");
            isInBuildingSpawn = true;
            animateBuildingSpawn = other.GetComponentInChildren<AnimateBuildingSpawn>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Building")
        {
            Debug.Log("Has left Building Spawn");
            isInBuildingSpawn = false;
            animateBuildingSpawn = null;
        }
    }

}
