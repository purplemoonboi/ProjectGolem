using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject endSceneCanvas;
    private float timer = 0f;
    private bool startTimer = false;

    private void Start()
    {
        endSceneCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ending scene in " + (5f - timer));
        if(startTimer)
        {
            timer += 1f * Time.deltaTime;
            if(timer >= 5f)
            {
                timer = 0f;
                startTimer = false;
                endSceneCanvas.SetActive(true);
            }
        }
    }

    public void StartTimer()
    {
        startTimer = true;
    }

}
