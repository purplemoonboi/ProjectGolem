using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackEnemyTarget : ActionNode
{
    public float distanceToTarget;

    public EnemyController enemyTarget;

    protected override void OnStart()
    {
        enemyTarget = blackboard.targetObj.GetComponent<EnemyController>();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (enemyTarget == null)
            return State.Failure;

        Vector3 direction = enemyTarget.transform.position - context.transform.position;
        distanceToTarget = direction.magnitude;

        if (context.friendlyController.GetHealth() <= (context.friendlyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (enemyTarget.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        if (distanceToTarget < 10.0f)
        {
            enemyTarget.SetHealth(enemyTarget.GetHealth() - context.friendlyController.GetPower());
        }
        else
            return State.Failure;

        return State.Running;
    }
}