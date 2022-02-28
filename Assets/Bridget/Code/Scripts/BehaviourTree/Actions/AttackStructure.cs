using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class AttackStructure : ActionNode
{
    //public float distanceToTarget;

    public EnemyTarget structure;

    protected override void OnStart()
    {
        if(blackboard.targetObj != null)
        {
            structure = blackboard.targetObj.GetComponent<EnemyTarget>();
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (structure == null)
            return State.Failure;
     
        //Vector3 direction = blackboard.targetObj.transform.position - context.transform.position;
        //distanceToTarget = direction.magnitude;

        if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        if (structure.GetHealth() <= 0.0f)
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        context.enemyController.SpawnProjectile(blackboard.targetObj.transform.position);

        return State.Running;
    }
}