using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyFlee : ActionNode
{
    public TimeController timeController;

    private System.TimeSpan currentTime;

    protected override void OnStart()
    {
        timeController = context.timeController.GetComponent<TimeController>();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Make enemy flee if it is dawn.
        currentTime = timeController.GetCurrentTime().TimeOfDay;

        if (
            (currentTime > timeController.GetSunrise() && currentTime < timeController.GetSunset())
            ||
            (context.enemyController.GetHealth() < (context.enemyController.GetMaxHealth() / 4.0f))
            )
        {
            Debug.Log("Returning to spawn...");
            blackboard.moveToPosition = context.enemyController.GetSpawnPoint();
            return State.Success;
        }
        else 
        {
            return State.Failure;
        }
    }
}