using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackStructure : ActionNode
{
    public TurretStats structure;

    protected override void OnStart()
    {
        if(blackboard.targetObj != null)
        {
            structure = blackboard.targetObj.GetComponent<TurretStats>();
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (structure == null)
            return State.Failure;

        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (structure.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        context.enemyController.SpawnProjectile(structure.transform.position);

        return State.Running;
    }
}