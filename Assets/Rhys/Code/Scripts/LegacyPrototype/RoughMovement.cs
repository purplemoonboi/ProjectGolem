using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoughMovement : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigidbody;

    private bool pressedW;
    private bool pressedS;
    private bool pressedA;
    private bool pressedD;

    [SerializeField]
    private Collider pickupCollider;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        pickupCollider = GetComponentInChildren<BoxCollider>();

        pressedW = false;
        pressedS = false;
        pressedA = false;
        pressedD = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            pressedW = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            pressedW = false;
        }
    
        if (Input.GetKey(KeyCode.S))
        {
            pressedS = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            pressedS = false;
        }
    
        if (Input.GetKey(KeyCode.D))
        {
            pressedD = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            pressedD = false;
        }
    
        if (Input.GetKey(KeyCode.A))
        {
            pressedA = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            pressedA = false;
        }

        Vector3 forward = new Vector3();

        if (pressedW)
        {
            transform.position += transform.forward * 15.0f * Time.deltaTime;
            forward = (transform.position + transform.forward);
        }
        else if (pressedS)
        {
            transform.position -= transform.forward * 15.0f * Time.deltaTime;
            forward = (transform.position - transform.forward);
        }
        else if (pressedD)
        {
            transform.position += transform.right * 15.0f * Time.deltaTime;
            forward = (transform.position + transform.right);
        }
        else if (pressedA)
        {
            transform.position -= transform.right * 15.0f * Time.deltaTime;
            forward = (transform.position - transform.right); 
        }

     // // Normalise diagonal movement
     // if (pressedW && pressedD)
     // {
     //     transform.position += Vector3.Normalize(transform.right + transform.forward) * 50.0f;
     // }
     // if (pressedW && pressedA)
     // {
     //     transform.position += Vector3.Normalize(-transform.right + transform.forward) * 50.0f;
     //
     // }
     // if (pressedS && pressedD)
     // {
     //     transform.position += Vector3.Normalize(transform.right + (-transform.forward)) * 50.0f;
     // }
     // if (pressedS && pressedA)
     // {
     //     transform.position += Vector3.Normalize(-(transform.right + transform.forward)) * 50.0f;
     // }

        

        //Camera.main.transform.forward = (transform.position + forward);

    }

    private void FixedUpdate()
    {
        

    }
  
}
