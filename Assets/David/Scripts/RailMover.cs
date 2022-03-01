using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : MonoBehaviour
{
    public Rail rail;

    public Transform lookAt;
    public bool smoothMove = true;
    public float moveSpeed = 5.0f;

    private Transform thisTransform;
    private Vector3 lastPosition;

    [SerializeField]
    private bool updateTarget = false;
    [SerializeField]
    private float turnSpeed = 4;
    [SerializeField]
    private Transform[] targets;
    [SerializeField]
    private Transform playerTransform;

    private float shortestDistance = 0.0f;

    [SerializeField]
    private Transform currentTarget;

    void Start()
    {
        thisTransform = transform;
        lastPosition = thisTransform.position;
        currentTarget = targets[0];

        Debug.Log("Target Length " + targets.Length);
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

        //Smooth camera look at.
        //Get the direction to the point 't'.
        Transform target = GetClosetTarget();

        Vector3 direction = (target.position - transform.position).normalized;
        //Create a quaternion of the goal.
        Quaternion rotationGoal = Quaternion.LookRotation(direction);
        //Update the camera rotation to follow point 't'.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, turnSpeed * Time.deltaTime);

    }

    private Transform GetClosetTarget()
    {
        shortestDistance = 0.0f;
        foreach(var target in targets)
        {
            float dist = Vector3.Distance(target.position, playerTransform.position);

            Debug.Log("Distance to target " + target.ToString() + " is " + dist + " Shortest distance is " + shortestDistance); 

            if (shortestDistance <= 0.1f || dist < shortestDistance)
            {
                Debug.Log("New target is " + target.ToString());

                shortestDistance = dist;
                currentTarget = target;
            }
        }

        return currentTarget;
    }

}
