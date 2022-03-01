using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
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
    private Transform[] azimuthTransform;

    [SerializeField]
    private EnemyTarget enemyTarget;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if(enemyTarget.IsActivated())
        {
            Debug.Log("Active!!!");
            if (target != null)
            {
                fireTimer += 1.0f * Time.deltaTime;

                if(fireTimer >= fireRate)
                {
                    StartCoroutine("RotateTurret");
                }
                else
                {
                    shouldFire = false;
                }
            }
        }
    }

    private IEnumerator RotateTurret()
    {
        Transform turretTransform = azimuthTransform[0];
        Vector3 turretDirection = turretTransform.forward;
        Vector3 epos = enemyTarget.transform.position;
        Vector3 direction = new Vector3();
        Quaternion rotationGoal = new Quaternion();

        while (azimuthTransform[0].rotation != rotationGoal)
        {
            direction = (target.position - transform.position).normalized;
            //Create a quaternion of the goal.
            rotationGoal = Quaternion.LookRotation(direction);
            //Update the camera rotation to follow point 't'.
            azimuthTransform[0].rotation = Quaternion.Slerp(azimuthTransform[0].rotation, rotationGoal, 2.0f * Time.deltaTime);

            yield return null;
        }

        shouldFire = true;
    }

    public void FixedUpdate()
    {
        if(enemyTarget.IsActivated())
        {
            if (shouldFire)
            {
                Debug.Log("FIRED PROJECTILE!");
                shouldFire = false;

                GameObject[] proj = new GameObject[2];
                proj[0] = Instantiate(prefab, barrelSpawns[0].position, Quaternion.identity);
                proj[1] = Instantiate(prefab, barrelSpawns[1].position, Quaternion.identity);

                Rigidbody[] rigidbody = { proj[0].GetComponent<Rigidbody>(), proj[1].GetComponent<Rigidbody>() };

                Vector3 direction = Vector3.Normalize(target.position - transform.position);
                Vector3 force = fireForce * direction;
                rigidbody[0].AddForce(force, ForceMode.Force);
                rigidbody[1].AddForce(force, ForceMode.Force);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(enemyTarget.IsActivated())
        {
            if (other.tag == "Enemy" && target == null)
            {
                target = other.transform;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(enemyTarget.IsActivated())
        {
            if (other.tag == "Enemy")
            {
                target = null;
            }
        }
    }

}
