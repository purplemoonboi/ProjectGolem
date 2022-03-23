using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackEnemyTarget : ActionNode
{
    public EnemyController enemyTarget = null;

    protected override void OnStart()
    {
        if (blackboard.targetObj != null)
        {
            enemyTarget = blackboard.targetObj.GetComponent<EnemyController>();

            enemyTarget.SetInCombat(true);
        }
    }

    protected override void OnStop()
    {
        if(enemyTarget != null)
        {
            enemyTarget.SetInCombat(false);
        }
    }

    protected override State OnUpdate()
    {
        if (blackboard.targetObj == null || enemyTarget == null)
            return State.Failure;

        if (context.friendlyController.GetHealth() <= (context.friendlyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (Vector3.Distance(context.transform.position, blackboard.targetObj.transform.position) > 12.0f)
            return State.Failure;

        if (enemyTarget.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        Vector3 direction = blackboard.targetObj.transform.position - context.transform.position;
        float singleStep = context.friendlyController.GetTurnSpeed() * Time.deltaTime;
        context.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(context.transform.forward, direction, singleStep, 0.0f));

        context.friendlyController.SpawnProjectile(blackboard.targetObj.transform.position);

        return State.Running;
    }
}