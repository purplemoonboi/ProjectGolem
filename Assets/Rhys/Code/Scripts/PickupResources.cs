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
    private GameObject currentResource;

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

        if(canPickupResource && pressedMouseB0 && currentResource)
        {
            Debug.Log("Picked up resource");
            resourceWallet += resourceAmount;
            resourceText.text = resourceWallet.ToString();
            Destroy(currentResource);
            currentResource = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Resource")
        {
            currentResource = other.gameObject;
            canPickupResource = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Resource")
        {
            currentResource = null;
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
