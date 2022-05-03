using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackFriendlyTarget : ActionNode
{
    public FriendlyController friendlyTarget = null;
    public TimeController timeController;

    private System.TimeSpan currentTime;
    private Transform targetTransform;

    protected override void OnStart()
    {
        timeController = context.timeController.GetComponent<TimeController>();

        if (blackboard.targetObj != null)
        {
            targetTransform = blackboard.targetObj.transform;
            friendlyTarget = blackboard.targetObj.GetComponent<FriendlyController>();

            if(friendlyTarget != null)
            friendlyTarget.SetInCombat(true);
        }
    }

    protected override void OnStop()
    {
        if(friendlyTarget != null)
        {
            friendlyTarget.SetInCombat(false);
        }    
    }

    protected override State OnUpdate()
    {
        if (blackboard.targetObj == null)
            return State.Failure;

        if (Vector3.Distance(context.transform.position, targetTransform.position) > 12.0f)
            return State.Failure;

        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;


        // Make enemy flee if it is dawn.
        currentTime = timeController.GetCurrentTime().TimeOfDay;

        if (currentTime > timeController.GetSunrise() && currentTime < timeController.GetSunset())
            return State.Failure;

        if(friendlyTarget != null)
        {
            if (friendlyTarget.GetHealth() <= 0.0f)
            {
                blackboard.targetObj = null;
                return State.Success;
            }
        }

        context.enemyController.SpawnProjectile(targetTransform.position);

        return State.Running;
    }
}