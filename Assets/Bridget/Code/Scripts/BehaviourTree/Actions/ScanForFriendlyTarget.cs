using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ScanForFriendlyTarget : ActionNode
{
    public TimeController timeController;

    private System.TimeSpan currentTime;

    protected override void OnStart()
    {

        if (timeController == null)
            return;

        timeController = context.timeController.GetComponent<TimeController>();

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        // Make enemy flee if it is dawn.
        currentTime = timeController.GetCurrentTime().TimeOfDay;

        if (currentTime > timeController.GetSunrise() && currentTime < timeController.GetSunset())
            return State.Failure;

        if (ScanTargets())
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
        var friendlyTargets = FindObjectsOfType<FriendlyController>();

        float distance = 0.0f;


        foreach (var target in friendlyTargets)
        {
            Vector3 direction = target.transform.position - context.transform.position;

            if (direction.magnitude < distance || distance == 0.0f)
            {
                distance = direction.magnitude;
                blackboard.moveToPosition = target.transform.position;
                blackboard.targetObj = target.gameObject;
            }
        }

        if (distance == 0.0f)
            return false;

        return true;
    }
}
