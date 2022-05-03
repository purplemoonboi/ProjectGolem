using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TurretAttackEnemy : ActionNode
{
    private Transform enemyTransform;

    private Transform turretAzimuth = null;
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

        enemyTransform = blackboard.targetObj.transform;

        GameObject thisTurret = context.gameObject;

        Transform[] azimuthTransforms = thisTurret.GetComponentsInChildren<Transform>();

        foreach (Transform transform in azimuthTransforms)
        {
            if (transform.tag == "TurretAzimuth")
            {
                if (turretAzimuth == null)
                {
                    turretAzimuth = transform;
                }

            }
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate() 
    {
        State nodeState = State.Running;

        if (context.turretGameObject.GetHealth() < 0f)
        {
            return State.Failure;
        }

        if (enemyTransform == null)
        {
            nodeState = State.Success;
            projectileManager.SetTarget(null);
        }
        else
        {
            Vector3 lookDirection = (enemyTransform.transform.position - turretAzimuth.position).normalized;
            Quaternion rotationGoal = Quaternion.LookRotation(lookDirection);

            if (enemy == null || Quaternion.Angle(turretAzimuth.rotation, rotationGoal) > 15f)
            {
                nodeState = State.Success;
                projectileManager.SetTarget(null);
            }
            else
            {
                projectileManager.SetTarget(enemy.transform);
            }
        }

        return nodeState;
    }
}
