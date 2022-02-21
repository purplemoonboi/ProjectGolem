using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @brief Animates the spawn of the building.
public class AnimateBuildingSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform endGoal;
    private bool movePlane;
    private bool hasSpawnedBuilding;
    private bool spawning;

    [SerializeField]
    private EnemyTarget tower;

    void Start()
    {
        movePlane = false;
        hasSpawnedBuilding = false;

        if (endGoal == null)
        {
            endGoal = GetComponentInChildren<Transform>();
        }

    }

    void Update()
    {
        if(movePlane && !hasSpawnedBuilding)
        {
            spawning = true;
            MovePlane();
        }
    }

 


    //Moves the plane towards the goal thus rendering the building as it
    //passes through the hologram.
    private void MovePlane()
    {
        Debug.Log("Animating buildings.");

        Vector3 currentPosition = transform.position;
        Vector3 direction = Vector3.Normalize(endGoal.position - currentPosition);

        currentPosition += direction * 10.0f * Time.deltaTime;

        if(Vector3.Distance(currentPosition, endGoal.position) < 0.001f)
        {
            movePlane = false;
            hasSpawnedBuilding = true;
            tower.SetIsActivated(true);
        }

        transform.position = currentPosition;
    }

    //This is called when the player enters the trigger.
    public void SetShouldMovePlane(bool value)
    {
        Debug.Log("Has triggered building");
        movePlane = value;
    }

    //This is also called if the player attempts to 
    //re-trigger the spawn.
    public bool HasAlreadySpawnedBuilding()
    {
        return hasSpawnedBuilding;
    }

    public bool IsSpawning()
    {
        return spawning;
    }
}
