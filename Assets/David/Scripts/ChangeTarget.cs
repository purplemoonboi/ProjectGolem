using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class ChangeTarget : MonoBehaviour
{
    public RailMover railMover;

    public Transform target;

    public Transform player;

    private void Update()
    {
        if (player == null)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            railMover.p = target;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        railMover.p = player;
    }
    
}
