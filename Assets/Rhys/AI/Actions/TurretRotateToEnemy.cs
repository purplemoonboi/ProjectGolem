using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TurretRotateToEnemy : ActionNode
{

    private Transform enemyTransform;

    private Transform turretAzimuth = null;
    private Transform turretAzimuthHolo = null;

    protected override void OnStart()
    {
        enemyTransform = blackboard.targetObj.transform;

        GameObject thisTurret = context.gameObject;

        Transform[] azimuthTransforms = thisTurret.GetComponentsInChildren<Transform>();

        foreach(Transform transform in azimuthTransforms)
        {
            if (transform.tag == "TurretAzimuth")
            {
                if (turretAzimuth == null)
                {
                    turretAzimuth = transform;
                }
                else if (turretAzimuthHolo == null)
                {
                    turretAzimuthHolo = transform;
                }
            }
        }
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {

        if (turretAzimuth == null || turretAzimuthHolo == null)
        {
            Debug.LogError("Turret's azimuth transform is null.");
            return State.Failure;
        }
        else
        {
            if (enemyTransform == null)
            {
                //Enemy was killed by other means.
                return State.Failure;
                Debug.LogError("Enemy transform is null. (Are they dead?)");
            }
            else
            {
                //Run procedure as normal.
                Vector3 lookDirection = (enemyTransform.transform.position - turretAzimuth.position).normalized;
                Quaternion rotationGoal = Quaternion.LookRotation(lookDirection);
                turretAzimuth.rotation = Quaternion.Slerp(turretAzimuth.rotation, rotationGoal, 2.0f * Time.deltaTime);
                turretAzimuthHolo = turretAzimuth;

                if (turretAzimuth.rotation != rotationGoal)
                {
                    Debug.Log("Rotating turret.");
                    return State.Running;
                }
                else
                {
                    Debug.Log("Target locked.");
                    return State.Success;
                }

            }
        }
    }
}
