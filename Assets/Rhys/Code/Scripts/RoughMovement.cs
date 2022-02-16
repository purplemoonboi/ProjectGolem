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

        Camera.main.transform.forward = transform.forward + transform.right;
    }

    private void FixedUpdate()
    {
        if (pressedW)
        {
            rigidbody.AddForce(transform.forward * 50.0f, ForceMode.Force);
        }
        else if (pressedS)
        {
            rigidbody.AddForce(-transform.forward * 50.0f, ForceMode.Force);
        }
        else if (pressedD)
        {
            rigidbody.AddForce(transform.right * 50.0f, ForceMode.Force);
        }
       else if (pressedA)
        {
            rigidbody.AddForce(-transform.right * 50.0f, ForceMode.Force);
        }

       // Normalise diagonal movement
       if (pressedW && pressedD)
       {
           rigidbody.AddForce(Vector3.Normalize(transform.right + transform.forward) * 50.0f, ForceMode.Force);
       }
       if (pressedW && pressedA)
       {
           rigidbody.AddForce(Vector3.Normalize(-transform.right + transform.forward) * 50.0f, ForceMode.Force);
       }
       if (pressedS && pressedD)
       {
           rigidbody.AddForce(Vector3.Normalize(transform.right + -transform.forward) * 50.0f, ForceMode.Force);
       }
       if (pressedS && pressedA)
       {
           rigidbody.AddForce(Vector3.Normalize(-transform.right + -transform.forward) * 50.0f, ForceMode.Force);
       }

    }
  
}
