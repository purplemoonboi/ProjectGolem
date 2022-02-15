using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupResources : MonoBehaviour
{

    [SerializeField]
    private bool pressedMouseB0;
    [SerializeField]
    private bool canPickupResource;
    [SerializeField]
    private Canvas canvas;

    private Text resourceText;
    [SerializeField]
    private int resourceWallet;
    [SerializeField]
    private int resourceAmount = 150;
    [SerializeField]
    private Material resourceMaterial;

    // Start is called before the first frame update
    void Start()
    {
        pressedMouseB0 = false;
        canPickupResource = false;
        resourceWallet = 0;
        resourceText = canvas.GetComponentsInChildren<Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressedMouseB0 = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressedMouseB0 = false;
        }

        if(canPickupResource && pressedMouseB0)
        {
            Debug.Log("Picked up resource");
            resourceWallet += resourceAmount;

            if(resourceMaterial)
            {
                resourceMaterial.SetColor("_Color", Color.red);
            }
            
            resourceText.text = resourceWallet.ToString();
        }
    }

    public void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Resource")
        {
            List<Material> otherMaterials = new List<Material>();
            trigger.GetComponentInChildren<Renderer>().GetMaterials(otherMaterials);
            resourceMaterial = otherMaterials[0];
            canPickupResource = true;
        }
    }

    public void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "Resource")
        {
            resourceMaterial = null;
            canPickupResource = false;
        }
    }

    public int GetResources()
    {
        return resourceWallet;
    }

    public void UpdateResources(int value)
    {
        resourceWallet += value;
    }

}
