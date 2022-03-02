using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class RailMover : MonoBehaviour
{
    public Rail rail;

    public Transform lookAt;
    public bool smoothMove = true;
    public float moveSpeed = 5.0f;

    public float speed = 1f;
    Quaternion newRot;
    Vector3 relPos;

    private Transform thisTransform;
    private Vector3 lastPosition;

    public Transform p;

    void Start()
    {
        thisTransform = transform;
        lastPosition = thisTransform.position;
        p = lookAt;
    }


    void LateUpdate()
    {
        if (smoothMove)
        {
            lastPosition = Vector3.Lerp(lastPosition, rail.ProjectPositionOnRail(lookAt.position), moveSpeed * Time.deltaTime);
            thisTransform.position = lastPosition;
        }
        else
        {
            thisTransform.position = rail.ProjectPositionOnRail(lookAt.position);

        }

        relPos = p.position - thisTransform.position;
        newRot = Quaternion.LookRotation(relPos);
        transform.rotation = Quaternion.RotateTowards(thisTransform.rotation, newRot, speed * Time.deltaTime);
        
        
        //thisTransform.LookAt(p);
    }
}
