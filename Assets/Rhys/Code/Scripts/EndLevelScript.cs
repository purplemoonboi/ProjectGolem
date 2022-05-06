using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject endSceneCanvas;
    [SerializeField]
    private float timer = 0f;
    [SerializeField]
    private bool startTimer = false;

    private void Start()
    {
        endSceneCanvas.SetActive(false);
    }

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
                endSceneCanvas.SetActive(true);
            }
        }
    }

    public void StartTimer()
    {
        startTimer = true;
    }

}
