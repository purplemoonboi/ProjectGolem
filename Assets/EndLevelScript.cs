using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float timer = 0f;
    private bool startTimer = false;

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            timer += 1f * Time.deltaTime;
            if(timer >= 5f)
            {
                timer = 0f;
                startTimer = false;
                SceneManager.LoadScene("WinScene");
            }
        }
    }

    public void ChangeLevel()
    {
        startTimer = true;
    }

}
