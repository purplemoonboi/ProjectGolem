using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackTarget : ActionNode
{
    public float distanceToTarget;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.target == null)
            return State.Failure;

        Vector3 direction = blackboard.target.transform.position - context.transform.position;
        distanceToTarget = direction.magnitude;

        //Debug.Log("Distance to Target: " + distanceToTarget);

        if (context.enemy.GetHealth() <= (context.enemy.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (blackboard.target.GetHealth() <= 0.0f)
            return State.Success;

        if (distanceToTarget < 10.0f)
        {
            blackboard.target.SetHealth(blackboard.target.GetHealth() - 1.0f);
            context.enemy.SetHealth(context.enemy.GetHealth() - 1.0f);
        }
        else
            return State.Failure;

        return State.Running;
    }
}
