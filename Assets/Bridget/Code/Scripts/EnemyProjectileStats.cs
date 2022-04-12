using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileStats : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float damage = 100.0f;
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private float timer = 0.0f;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this != null)
        {
            timer += 1.0f * Time.deltaTime;
            if (timer > 3.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FixedUpdate()
    {

    }

    public void ApplyForce(Vector3 force)
    {
        rigidbody.AddForce(force, ForceMode.Force);
    }

  // public void OnCollisionEnter(Collision collision)
  // {
  //     if (collision.transform.tag == "DefenceTower")
  //     {
  //         EnemyTarget target = collision.transform.GetComponent<EnemyTarget>();
  //
  //         if (target != null)
  //         {
  //             target.SetHealth(target.GetHealth() - damage);
  //
  //             Destroy(gameObject);
  //         }
  //     }
  //
  //     else if (collision.transform.tag == "Friendly")
  //     {
  //         FriendlyController friendly = collision.transform.GetComponent<FriendlyController>();
  //
  //         if (friendly != null)
  //         {
  //             friendly.SetHealth(friendly.GetHealth() - damage);
  //
  //             Destroy(gameObject);
  //         }
  //     }
  // }
  //
  // public void OnCollisionExit(Collision collision)
  // {
  //     if (collision.transform.tag == "DefenceTower" || collision.transform.tag == "Friendly")
  //     {
  //         if (this != null)
  //         {
  //             Destroy(gameObject);
  //         }
  //     }
  // }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.transform.tag == "Friendly")
        {
            FriendlyController friendly = collider.gameObject.transform.GetComponent<FriendlyController>();

            if (friendly != null)
            {
                friendly.SetHealth(friendly.GetHealth() - damage);

                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
        else if(collider.gameObject.transform.tag == "DefenceTower")
        {
            TurretStats turret = collider.gameObject.transform.GetComponent<TurretStats>();

            if(turret != null)
            {
                turret.SetHealth(turret.GetHealth() - damage);

                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.transform.tag == "DefenceTower" || collider.gameObject.transform.tag == "Friendly")
        {
            if (this != null)
            {
                Destroy(gameObject);
            }
        }
    }
}