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
    private float fireForce = 100.0f;
    [SerializeField]
    private float fireRate = 4.0f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool shouldFire = false;

    private float fireTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            fireTimer += 1.0f * Time.deltaTime;

            if (fireTimer >= fireRate)
            {
                shouldFire = true;
                fireTimer = 0.0f;
            }
        }
    }

    public void FixedUpdate()
    {
        if(shouldFire)
        {
            shouldFire = false;
            GameObject[] proj = { Instantiate(prefab, barrelSpawns[0]), Instantiate(prefab, barrelSpawns[1]) };
            Rigidbody[] rigidbody = { proj[0].GetComponent<Rigidbody>(), proj[1].GetComponent<Rigidbody>() };
            Vector3 direction = Vector3.Normalize(target.position - transform.forward);
            Vector3 force = fireForce * direction;
            rigidbody[0].AddForce(force, ForceMode.Force);
            rigidbody[1].AddForce(force, ForceMode.Force);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            target = other.transform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            target = null;
        }
    }

}
