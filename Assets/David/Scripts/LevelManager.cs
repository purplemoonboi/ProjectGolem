using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject player;
    public Transform respawnPoint;

    [SerializeField] private GameObject cam1;
    [SerializeField] private GameObject cam2;

    private bool frontCam = true;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.transform.position;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SwitchPriority();
        }
    }

    private void SwitchPriority()
    {
        if (frontCam)
        {
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
        else
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
        }
        frontCam = !frontCam;
    }
}
