using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class EnemyFlee : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.enemyController.GetHealth() > (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        blackboard.moveToPosition = context.enemyController.GetSpawnPoint();

        return State.Success;
    }
}