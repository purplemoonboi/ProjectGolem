using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TurretInactive : ActionNode
{

    private EnemyTarget turretStatistics;
    private Quaternion rotationGoal;
    private Transform activatedTransform;
    private Transform turretAzimuth = null;
    private Transform turretAzimuthHolo = null;

    protected override void OnStart() 
    {
        turretStatistics = context.gameObject.GetComponent<EnemyTarget>();

        GameObject thisTurret = context.gameObject;

        //Set up the azimuth transforms.
        Transform[] transforms = thisTurret.gameObject.GetComponentsInChildren<Transform>();
        
        foreach(Transform transform in transforms)
        {
            if(transform.tag == "ActivatedTurretTransform")
            {
                activatedTransform = transform;
            }
            if(transform.tag == "TurretAzimuth")
            {
                if(turretAzimuth == null)
                {
                    turretAzimuth = transform;
                }
                else if(turretAzimuthHolo == null)
                {
                    turretAzimuthHolo = transform;
                }
            }
        }
     
        if(activatedTransform == null)
        {
            Debug.LogError("Could not find turret activated transform object.");
        }

        //Set up rotation goal on start.
        Vector3 lookDirection = (activatedTransform.position - turretAzimuth.position).normalized;
        rotationGoal = Quaternion.LookRotation(lookDirection);
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate()
    {
        if(turretStatistics.IsActivated())
        {
            if(ActivateTurretAnimation())
            {
                Debug.Log("Turret Activated.");
                return State.Success;
            }
        }


        return State.Running;
    }

    private bool ActivateTurretAnimation()
    {
        turretAzimuth.rotation = Quaternion.Slerp(turretAzimuth.rotation, rotationGoal, 2.0f * Time.deltaTime);
        return (turretAzimuth.rotation == rotationGoal) ? true : false;
    }

}
