using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyHealSelf : ActionNode
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

        if (currentTime > timeController.GetSunrise() && currentTime < timeController.GetSunset())
        {
            Debug.Log("Enemy Flee!");
            context.enemyController.SetHealth(-999f);
            return State.Success;
        }

        if (context.enemyController.GetHealth() >= context.enemyController.GetMaxHealth())
        {
            return State.Success;
        }

        context.enemyController.SetHealth(context.enemyController.GetHealth() + 1.0f);

        return State.Running;
    }
}
