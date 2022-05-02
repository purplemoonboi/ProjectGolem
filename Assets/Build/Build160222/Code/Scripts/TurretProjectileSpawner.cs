using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Transform[] barrelSpawns;
    [SerializeField]
    private float fireForce = 1000.0f;
    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool shouldFire = false;
    [SerializeField]
    private bool azimuthAligned = false;
    [SerializeField]
    private float fireTimer = 0.0f;
    [SerializeField]
    private Transform azimuthTransform;
    [SerializeField]
    private TurretStats turretStatistics;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if(turretStatistics.IsActivated())
        {
            if (target != null)
            {
                fireTimer += 1.0f * Time.deltaTime;

                if (fireTimer > fireRate)
                {
                    fireTimer = 0.0f;
                    shouldFire = true;
                }
            }
        }
    }

    private IEnumerator RotateTurret()
    {
        Transform turretTransform = azimuthTransform;
        Vector3 turretDirection = turretTransform.forward;
        Vector3 epos = turretStatistics.transform.position;

        while (turretDirection != epos)
        {
            Vector3 newForward = Vector3.RotateTowards(turretTransform.forward, epos, 2.0f * Time.deltaTime, 0.0f);
            turretTransform.forward = newForward;

            yield return null;
        }

        shouldFire = true;
    }

    public void FixedUpdate()
    {
        if (target == null)
            return;

        if (turretStatistics.IsActivated())
        {
            if (shouldFire)
            {
                shouldFire = false;

                GameObject proj = new GameObject();
                Quaternion rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
                if (turretStatistics.Level < 2)
                {
                    proj = Instantiate(prefab, barrelSpawns[0].position, rotation);
                }
                else
                {
                    foreach (var spawn in barrelSpawns)
                    {
                        proj = Instantiate(prefab, spawn.position, rotation);
                    }
                }

                Rigidbody rigidbody = proj.GetComponent<Rigidbody>();

                Vector3 direction = Vector3.Normalize(target.position - transform.position);
                Vector3 force = fireForce * direction;
                rigidbody.AddForce(force, ForceMode.Force);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (turretStatistics.IsActivated())
        {
            if (other.tag == "Enemy" && target == null)
            {
                target = other.transform;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (turretStatistics.IsActivated())
        {
            if (other.tag == "Enemy")
            {
                target = null;
            }
        }
    }


    public void SetTarget(Transform newTarget) => target = newTarget;
}
