using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyHealSelf : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if(context.enemyController.GetHealth() >= context.enemyController.GetMaxHealth())
        {
            return State.Success;
        }

        context.enemyController.SetHealth(context.enemyController.GetHealth() + 1.0f);

        return State.Running;
    }
}
