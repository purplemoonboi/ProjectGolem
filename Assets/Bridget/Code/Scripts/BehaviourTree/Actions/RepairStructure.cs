using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepairStructure : ActionNode
{
    public float distanceToTarget;

    public EnemyTarget structure;

    private float timer = 0.0f;

    protected override void OnStart()
    {
        if (blackboard.targetObj != null)
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
            PerformRepair();
        }
        else
            return State.Failure;

        return State.Running;
    }

    public void PerformRepair()
    {
        timer += 1.0f * Time.deltaTime;

        if (timer > 0.8f)
        {
            structure.SetHealth(structure.GetHealth() + context.friendlyController.GetPower());
            timer = 0.0f;
        }
    }
}