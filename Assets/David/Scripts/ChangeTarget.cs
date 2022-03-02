using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class ChangeTarget : MonoBehaviour
{
    public RailMover railMover;

    private Camera cam;

    public Transform target;

    public Transform player;


    public bool isOn = false;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (isOn)
        {
            cam.transform.LookAt(target.position);
        }
        else
        {
            cam.transform.LookAt(railMover.lookAt.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            // cam.transform.LookAt(target.position);
            // Debug.Log(target.name);

            isOn = true;
            Debug.Log(target.name);
            Debug.Log("Enter!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit!");
        isOn = false;
    }
}
