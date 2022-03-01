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

    private Transform thisTransform;
    private Vector3 lastPosition;

    void Start()
    {
        thisTransform = transform;
        lastPosition = thisTransform.position;
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
        //thisTransform.LookAt(lookAt.position);
    }
}
