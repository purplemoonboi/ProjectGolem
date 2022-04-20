using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TurretAttackEnemy : ActionNode
{

    private GameObject enemy;
    private TurretProjectileSpawner projectileManager;

    protected override void OnStart() 
    {
        enemy = blackboard.targetObj;

        TurretProjectileSpawner fireProjectile = context.gameObject.GetComponentInChildren<TurretProjectileSpawner>();
        if (fireProjectile != null)
        {
            projectileManager = fireProjectile;
        }
        else
        {
            Debug.LogError("Turret AI : Could not find 'FireProjectile' script in context children.");
        }

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate() 
    {
        State nodeState = State.Running;

        if(enemy == null)
        {
            nodeState = State.Success;
            projectileManager.SetTarget(null);
        }
        else
        {
            projectileManager.SetTarget(enemy.transform);
        }

        return nodeState;
    }
}
