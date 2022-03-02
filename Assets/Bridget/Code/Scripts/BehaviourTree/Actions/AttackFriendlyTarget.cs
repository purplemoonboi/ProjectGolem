using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackFriendlyTarget : ActionNode
{
    public float distanceToTarget;

    public FriendlyController friendlyTarget;

    protected override void OnStart()
    {
        if (blackboard.targetObj != null)
        {
            friendlyTarget = blackboard.targetObj.GetComponent<FriendlyController>();


        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (friendlyTarget == null)
            return State.Failure;

        //Vector3 direction = friendlyTarget.transform.position - context.transform.position;
        //distanceToTarget = direction.magnitude;

        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (friendlyTarget.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        context.enemyController.SpawnProjectile(friendlyTarget.transform.position);

        return State.Running;
    }
}
