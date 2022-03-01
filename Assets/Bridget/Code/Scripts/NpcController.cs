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
    protected float health;
    protected float maxHealth;
    protected float power;

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

        float newWidth = Remap(health, 0.0f, maxHealth, 0.0f, maxWidth);
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
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
    }

    public void SetHealth(float h) { health = h; }

    public float GetHealth() { return health; }

    public float GetMaxHealth() { return maxHealth; }

    public void SetPower(float p) { power = p; }

    public float GetPower() { return power; }
}