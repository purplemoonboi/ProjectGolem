using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ScanForFriendlyTarget : ActionNode
{
    public TimeController timeController;

    private System.TimeSpan currentTime;

    private GameObject player;

    protected override void OnStart()
    {
        timeController = context.timeController.GetComponent<TimeController>();
        player = GameObject.FindGameObjectWithTag("Player");
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

        bool targetFound;
        float distanceToTarget;

        (targetFound, distanceToTarget) = ScanTargets();

        if (targetFound)
        {
            if (distanceToTarget > Vector3.Distance(player.transform.position, context.transform.position))
                blackboard.targetObj = player;

            return State.Success;
        }

        else
        {
            blackboard.targetObj = player;
            return State.Success;
        }

        return State.Running;
    }

    public (bool, float) ScanTargets()
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
            return (false, 0.0f);

        return (true, distance);
    }
}
