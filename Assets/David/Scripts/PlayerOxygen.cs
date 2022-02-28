using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
    public static PlayerOxygen instance;

    public Slider oxygenBar;
    public float oxygenAmount = 100.0f;
    public float currentOxygen;
    public float reduceAmount = 5.0f;

    private GameObject Player;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        currentOxygen = oxygenAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
            return;

        LoseOxygen();
        PlayerDeath();
    }

    public void LoseOxygen()
    {
        if (RecoverOxygen.instance.isRecovering == false)
        {
            currentOxygen -= reduceAmount * Time.deltaTime;
            oxygenBar.value = currentOxygen / oxygenAmount;
        }
    }

    void PlayerDeath()
    {
        if (currentOxygen <= 0f)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0f);
        LevelManager.instance.Respawn();
        currentOxygen = oxygenAmount;
    }
}
