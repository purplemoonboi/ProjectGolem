using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarricadeBuilding : Building
{
    [SerializeField]
    private BarricadeScriptableObject barricadeScriptableObject;

    [SerializeField]
    private Transform shieldTransform;
    [SerializeField]
    private Vector3 initialScale;
    [SerializeField]
    private BoxCollider collider;

    private void Awake()
    {
        initialScale = shieldTransform.localScale;
        shieldTransform.localScale = new Vector3(0f, 0f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBuildingType(BuildingType.Barricade);
        ResetParameters();
        barricadeScriptableObject.isMaxLevel = false;
        barricadeScriptableObject.level = 1;
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
         if (ShouldSpawn())
         {
             Debug.Log("Now spawning building.");
             StartCoroutine("PlaySpawnAnimation");
            collider.enabled = true;
         }
    }

    protected override IEnumerator PlaySpawnAnimation() 
    {
        Debug.Log("Animating buildings.");
        Debug.Log("Scale " + transform.localScale.x);

        while (shieldTransform.localScale.x < initialScale.x)
        {
            shieldTransform.localScale += new Vector3(0.8f, 0.8f, 0.8f) * Time.deltaTime;
            
            yield return null;
        }

        //The building has now spawned.
        shouldSpawn = false;
        isActive = true;
    }

    public override void Upgrade()
    {
        IncrimentBuildingLevel();
        SetCostToUpgrade(GetCostToUpgrade());
        SetMaxHealth((int)GetHealth() + 100);

    }

    /*..Trigger callback methods..*/

    public void OnTriggerEnter(Collider other)
    {
        // if (other.tag == "Player")
        // {
        //     BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
        //     buildingInfo.EnableInfoPanel();
        //
        //     string[] infoArray =
        //     {
        //          barricadeScriptableObject.prefabName.ToString(),
        //          "Health" + health.ToString(),
        //          "Level " + GetLevel().ToString(),
        //          (!isActive) ? "Cost to build " + GetCost().ToString() : "Cost to upgrade " + GetCostToUpgrade().ToString(),
        //          buildingType.ToString()
        //     };
        //
        //     Text infoText = GetComponentInChildren<Text>();
        //     infoText.text = " ";
        //
        //     for (int i = 0; i < infoArray.Length; ++i)
        //     {
        //         infoText.text = infoText.text + "\n" + infoArray[i];
        //     }
        //
        //     buildingInfo.SetText(infoText);
        // }
        if (other.tag == "Player")
        {
            if (!other.GetComponent<Interact>().IsTalking())
            {
                this.buildingInfoCanvas.SetActive(true);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // BuildingInfoPanel buildingInfo = GetComponentInChildren<BuildingInfoPanel>();
            // buildingInfo.DisableInfoPanel();
            // Text infoText = GetComponentInChildren<Text>();
            // infoText.text = " ";
            this.buildingInfoCanvas.SetActive(false);

        }
    }

    protected void IncrimentBuildingLevel() => barricadeScriptableObject.level++;

    public void SetMaxHealth(int _health) => barricadeScriptableObject.maximumHealth += _health;

    public bool IsMaxLevel() => barricadeScriptableObject.isMaxLevel;

    public int GetLevel() => barricadeScriptableObject.level;

    public int GetMaxLevel() => barricadeScriptableObject.maxLevel;

}
