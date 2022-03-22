//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private float elapsedTime;
    private float attackTime;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private float health;
    private const float MAX_HEALTH = 100;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject healthText;

    [SerializeField]
    private GameObject healthBar;
    private RectTransform healthBarRect;

    [SerializeField]
    private float maxWidth;

    [SerializeField]
    private float damageTaken;

    [SerializeField]
    private TurretStats target;

    private NavMeshAgent agent;

    [SerializeField]
    private int id;
    [SerializeField]
    private int power;  //How much damage the enemy can do in one hit

    [SerializeField]
    private List<TurretStats> targetsInProximity;
    [SerializeField]
    private bool shouldUpdateTarget;

    private bool reflectDirection;

    [SerializeField]
    private Vector3 patrolDirection;

    void Start()
    {
        health = MAX_HEALTH;
        attackTime = 1.0f;
        elapsedTime = attackTime;
        healthText.GetComponent<Text>().text = "HEALTH: " + health;
        healthBarRect = healthBar.GetComponent<RectTransform>();
        maxWidth = healthBarRect.rect.width;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.forward;
        power = 10;

        //Initialise list.
        targetsInProximity = new List<TurretStats>();
        shouldUpdateTarget = true;
        reflectDirection   = false;
        target             = null;
        patrolDirection    = transform.forward;
    }

    void Update()
    {
        UpdateCanvasRotation();

        if (health <= 0.0f)
        {
           // agent.enabled = false;
           // transform.position.Set(-999.9f, -999.9f, -999.9f);  //Moving the object far away so OnCollisionExit will trigger
           //
           // isDead = true;
            Destroy(gameObject);
        }

        if(target != null)
        {
            if(agent.enabled)
            {
                agent.destination = target.transform.position;
            }
        }
       else
       {
           if(reflectDirection)
           {
               patrolDirection = Vector3.RotateTowards(patrolDirection, -patrolDirection, Mathf.Deg2Rad * 30.0f, 360.0f);
               reflectDirection = false;
           }
      
           agent.destination = patrolDirection * 10.0f;
       }

        if (damageTaken > 0.0f)
        {
            ProcessDamage();
        }
    }

    private void FixedUpdate()
    {
        Vector3 origin, direction;
        Ray ray;
        RaycastHit hit;
        float maxDistance = 50;

        origin = transform.position;
        direction = transform.forward;
        ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out hit, maxDistance, (1 << 6)))
        {
            // EnemyTarget tower = hit.collider.GetComponent<EnemyTarget>();

            // if(tower)
            // {
            //     target = tower.gameObject;
            // }

            if (Vector3.Distance(hit.transform.position, transform.position) < 2.0f)
            {
                reflectDirection = true;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DefenceTower")
        {
            TurretStats targetReference = other.gameObject.GetComponent<TurretStats>();
            if(targetReference.IsActivated())
            {
                if (shouldUpdateTarget)
                {
                    target = other.GetComponent<TurretStats>();
                    shouldUpdateTarget = false;
                }
            }
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
       // EnemyTarget tower = collision.transform.gameObject.GetComponent<EnemyTarget>();

        if (target == collision.gameObject)
        {
            Debug.Log(gameObject.name + " IS colliding with " + collision.gameObject.name);

            damageTaken += Mathf.Max(0, target.GetPower());
        }
    }

    public void OnCollisionExit(Collision collision)
    {
       // EnemyTarget tower = collision.transform.gameObject.GetComponent<EnemyTarget>();

        if (target == collision.gameObject)
        {
            Debug.Log(gameObject.name + " IS NOT colliding with " + collision.gameObject.name);

            damageTaken -= Mathf.Max(0, target.GetPower());  //Ensuring negative damage can't be done

            if (isDead)
            {
                target = null;
                shouldUpdateTarget = true;
                Destroy(gameObject);
            }
        }
    }

    public void ProcessDamage()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > attackTime)
        {
            elapsedTime = 0.0f;

            health -= damageTaken;
            Mathf.Clamp(health, 0, MAX_HEALTH);

            healthText.GetComponent<Text>().text = "HEALTH: " + health;

            float newWidth = Remap(health, 0.0f, MAX_HEALTH, 0.0f, maxWidth);
            healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }

    public void UpdateCanvasRotation()
    {
        Vector3 direction = canvas.transform.position - Camera.main.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        canvas.transform.rotation = lookRotation;
    }

    private float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
    }

    public void SetID(int enemyID) { id = enemyID; }

    public int GetID() { return id; }

    public int GetPower() { return power; }

    public void UpdateDamageTaken(float value)
    {
        health -= value;
    }

    public float GetCurrentDamageTaken()
    {
        return damageTaken;
    }
}