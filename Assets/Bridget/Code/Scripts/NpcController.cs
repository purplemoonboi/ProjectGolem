using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Base controller class that all AI-controlled characters inherit from. This includes Enemy and Friendly characters.
public class NpcController : MonoBehaviour
{
    [SerializeField]
    private GameObject healthText;

    [SerializeField]
    private GameObject healthBar;
    private RectTransform healthBarRect;

    protected float maxWidth;
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float maxHealth;
    [SerializeField]
    protected bool alive = true;
    [SerializeField]
    protected float power;
    [SerializeField]
    protected Vector3 spawnPoint = new Vector3(0.0f, 1.0f, 0.0f);
    [SerializeField]
    protected bool inCombat = false;
    [SerializeField]
    protected float turnSpeed = 15.0f;

    void Start()
    {
    }

    void Update()
    {
        UpdateUIComponents();
        CheckDeath();
    }

    public void UpdateUIComponents()
    {
        healthText.GetComponent<Text>().text = "HEALTH: " + health;

        float newWidth = MathsUtils.RemapRange(health, 0.0f, maxHealth, 0.0f, maxWidth);
        healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    protected void SetupCharacter(float maxH, float pow)
    {
        maxHealth = maxH;
        health = maxHealth;
        power = pow;

        healthText.GetComponent<Text>().text = "HEALTH: " + health;
        healthBarRect = healthBar.GetComponent<RectTransform>();
        maxWidth = healthBarRect.rect.width;
    }

    protected void CheckDeath()
    {
        if (health < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetHealth(float h) { health = h; }

    public float GetHealth() { return health; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetPower(float p) { power = p; }

    public float GetPower() { return power; }

    public void SetSpawnPoint(Vector3 spawn) { spawnPoint = spawn; }

    public Vector3 GetSpawnPoint() { return spawnPoint; }

    public void SetInCombat(bool combat) { inCombat = combat; }

    public bool GetInCombat() { return inCombat; }

    public void SetTurnSpeed(float speed) { turnSpeed = speed; }

    public float GetTurnSpeed() { return turnSpeed; }

    public bool GetAlive() { return alive; }
}