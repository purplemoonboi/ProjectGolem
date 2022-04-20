using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackFriendlyTarget : ActionNode
{
    public FriendlyController friendlyTarget = null;
    public TimeController timeController;

    private System.TimeSpan currentTime;

    protected override void OnStart()
    {
        timeController = context.timeController.GetComponent<TimeController>();
        if (blackboard.targetObj != null)
        {
            friendlyTarget = blackboard.targetObj.GetComponent<FriendlyController>();

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
        if (blackboard.targetObj == null || friendlyTarget == null)
            return State.Failure;

        if (Vector3.Distance(context.transform.position, friendlyTarget.transform.position) > 12.0f)
            return State.Failure;

        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;


        // Make enemy flee if it is dawn.
        currentTime = timeController.GetCurrentTime().TimeOfDay;

        if (currentTime > timeController.GetSunrise() && currentTime < timeController.GetSunset())
            return State.Failure;

        if (friendlyTarget.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        //Vector3 direction = friendlyTarget.gameObject.transform.position - context.transform.position;
        //float singleStep = context.enemyController.GetTurnSpeed() * Time.deltaTime;
        //context.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(context.transform.forward, direction, singleStep, 0.0f));

        context.enemyController.SpawnProjectile(friendlyTarget.transform.position);

        return State.Running;
    }
}