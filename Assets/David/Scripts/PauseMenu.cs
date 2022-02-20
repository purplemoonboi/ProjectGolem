using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//@author David Costa

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    //private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //void ChangeStates()
    //{
    //    if (!isPaused)
    //    {
    //        isPaused = true;
    //        pauseMenu.SetActive(true);
    //        Time.timeScale = 0f;
    //    }
    //    else
    //    {
    //        isPaused = false;
    //        pauseMenu.SetActive(false);
    //        Time.timeScale = 1f;
    //    }
    //    isPaused = !isPaused;
    //}

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
