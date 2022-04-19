using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BuildingType
{
    None = -1,
    Builder = 0,
    Weapons = 1,
    Barricade = 2,
    Camp = 3,
    Turret = 4
}

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    private GameObject holoGFX;
    [SerializeField]
    protected int cost;
    [SerializeField]
    protected int costToUpgrade;
    [SerializeField]
    protected float health = 1000.0f;
    [SerializeField]
    protected bool isActive = false;
    [SerializeField]
    protected bool shouldSpawn = true;
    [SerializeField]
    protected BuildingType buildingType = BuildingType.None;

    private void Start()
    {

    }

    //Moves the plane towards the goal thus rendering the building as it
    //passes through the hologram.
    protected virtual IEnumerator PlaySpawnAnimation()
    {
       // //Debug.Log("Animating buildings.");
       //
       // while (Vector3.Distance(meshCutterTransform.position, spawnerEndGoal.position) > 0.001f)
       // {
       //     meshCutterTransform.position = Vector3.Lerp(meshCutterTransform.position, spawnerEndGoal.position, 1.0f * Time.deltaTime);
       //
       //     //Returns from this function to the main engine loop.
       //     //Will re-visit this function until the condition has
       //     //been met.
       //     yield return null;
       // }

        //The building has now spawned.
        shouldSpawn = false;
        isActive = true;
        yield return null;
    }

    /*..Abstract methods..*/

    // @brief All buildings contain an overriden upgrade method.
    public abstract void Upgrade();

    /*..Public Setters..*/

    public void Spawn()
    {
        shouldSpawn = true;
        Destroy(holoGFX);
    }

    public virtual void SetCost(int _cost) => cost = _cost;

    public virtual void SetCostToUpgrade(int _costToUpgrade) => costToUpgrade += _costToUpgrade;

    public void SetBuildingType(BuildingType type)
    {
        buildingType = type;
        Debug.Log("Building type " + ToString());

    }

    public void ResetParameters()
    {
        isActive = false;
        shouldSpawn = false;
    }

    public void BuildingCosts(int _cost, int _costToUpgrade)
    {
        cost = _cost;
        costToUpgrade = _costToUpgrade;
    }

    /*..Public Getters..*/

    public float GetHealth() => health;

    public bool ShouldSpawn() => shouldSpawn; 

    public virtual int GetCost() => cost; 

    public BuildingType GetBuildingType() => buildingType; 
    
    public bool IsActive() => isActive; 

    public virtual int GetCostToUpgrade() => costToUpgrade; 
                                                                            
}
