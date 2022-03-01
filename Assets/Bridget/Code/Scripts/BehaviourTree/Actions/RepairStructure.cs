using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepairStructure : ActionNode
{
    public float distanceToTarget;

    public EnemyTarget structure;

    protected override void OnStart()
    {
        structure = blackboard.targetObj.GetComponent<EnemyTarget>();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (structure == null)
            return State.Failure;

        Vector3 direction = blackboard.targetObj.transform.position - context.transform.position;
        distanceToTarget = direction.magnitude;

        if (structure.GetHealth() >= structure.GetMaxHealth())
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        if (distanceToTarget < 10.0f)
        {
            structure.SetHealth(structure.GetHealth() + context.friendlyController.GetPower());
        }
        else
            return State.Failure;

        return State.Running;
    }
}
