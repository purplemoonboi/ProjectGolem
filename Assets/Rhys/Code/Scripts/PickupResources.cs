using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupResources : MonoBehaviour
{

    [SerializeField]
    private bool pressedMouseB0;

    // Start is called before the first frame update
    void Start()
    {
        pressedMouseB0 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pressedMouseB0 = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressedMouseB0 = false;
        }
    }

    public void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Resource")
        {
            if(pressedMouseB0 == true)
            {
                Debug.Log("Picked up some radioactive sand!");
            }
        }
    }

}
