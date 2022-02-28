using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class RecoverOxygen : MonoBehaviour
{
    public static RecoverOxygen instance;

    public float recoverAmount = 1.5f;

    public bool isRecovering = false;

    private void Awake()
    {
        instance = this;
    }


    private void Update()
    {
        if (isRecovering)
        {
            Recover();
            Debug.Log(PlayerOxygen.instance.currentOxygen);
        }
        else
        {
            PlayerOxygen.instance.LoseOxygen();
        }

        if(PlayerOxygen.instance.currentOxygen > 100)
        {
            PlayerOxygen.instance.currentOxygen = 100.0f;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerOxygen.instance.currentOxygen < 100f)
            isRecovering = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isRecovering = false;
    }

    private void Recover()
    {
        PlayerOxygen.instance.currentOxygen += recoverAmount * Time.deltaTime;
        PlayerOxygen.instance.oxygenBar.value = PlayerOxygen.instance.currentOxygen / PlayerOxygen.instance.oxygenAmount;
    }
}
