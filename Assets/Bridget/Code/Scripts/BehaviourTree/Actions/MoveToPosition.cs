using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToPosition : ActionNode
{
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;

    protected override void OnStart()
    {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.SetDestination(blackboard.moveToPosition);
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

        context.agent.isStopped = false;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.agent.pathPending)
        {
            //Debug.Log("Path for " + context.transform.name + "is PENDING");
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance)
        {
            context.agent.isStopped = true;
            //Debug.Log(context.transform.name + " is STOPPED!");

            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)//|| context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
        {
            return State.Failure;
        }

        if (blackboard.targetObj != null)
        {
            blackboard.moveToPosition = blackboard.targetObj.transform.position;
            context.agent.SetDestination(blackboard.moveToPosition);
        }

        return State.Running;
    }
}