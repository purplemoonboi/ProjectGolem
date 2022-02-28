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

        blackboard.moveToPosition = new Vector3(0.0f, 1.0f, 0.0f);

        return State.Success;
    }
}
