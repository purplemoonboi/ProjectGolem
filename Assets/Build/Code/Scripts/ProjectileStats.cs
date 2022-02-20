using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{

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
        if(this != null)
        {
            timer += 1.0f * Time.deltaTime;
            if(timer > 6.0f)
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

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            EnemyMovement enemy = collision.transform.GetComponent<EnemyMovement>();

            if(enemy != null)
            {
                enemy.UpdateDamageTaken(damage);

                Destroy(gameObject);

            }

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if(this != null)
            {
                Destroy(gameObject);

            }
        }
    }


}
