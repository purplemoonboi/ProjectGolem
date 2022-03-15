using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ScanForTargets : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(ScanTargets())
        {
            return State.Success;
        }

        else
        {
            return State.Failure;
        }

        return State.Running;
    }

    public bool ScanTargets()
    {
        var enemyTargets = FindObjectsOfType<TurretStats>();

        float distance = 0.0f;

        foreach(var target in enemyTargets)
        {
            Vector3 direction = target.transform.position - context.transform.position;

            if (direction.magnitude < distance || distance == 0.0f)
            {
                distance = direction.magnitude;
                blackboard.moveToPosition = target.transform.position;
                //blackboard.target = target;
            }

            //Debug.Log("Target Pos: " + target.transform.position + "Distance:" + distance);
        }

        //Debug.Log("Next Position: " + blackboard.moveToPosition);

        if (distance == 0.0f)
            return false;

        return true;
    }
}
