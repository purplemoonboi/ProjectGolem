using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private Text typerwriterText;
    [SerializeField] private Text skipMessageText;

    [SerializeField] private List<string> tutorialMessages = new List<string>();

    [SerializeField] private float printDelay = 0.033f;
    [SerializeField] private KeyCode skipKey;
    [SerializeField] private int msgIndex = 0;
    [SerializeField] private bool runningSequence = false;

    private ThirdPersonController playerController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
            playerController = other.gameObject.GetComponent<ThirdPersonController>();
            
            if(playerController)
            {
                playerController.DisableInput = true;
            }

            StartSequence();
        }
    }

    void Update()
    {
        if (runningSequence)
        {
            //Updating the skip/continue text
            if (TypewriterEffect.isPrinting)
                skipMessageText.text = "'" + skipKey.ToString() + "' to skip";
            else
                skipMessageText.text = "'" + skipKey.ToString() + "' to continue";

            //Checking if the skip/continue button has been pressed and deciding what to print accordingly
            if (Input.GetKeyDown(skipKey) || msgIndex == 0)
            {
                if (TypewriterEffect.isPrinting)
                {
                    TypewriterEffect.skippedDialogue = true;
                }
                else
                {
                    if (msgIndex < tutorialMessages.Count)
                    {
                        StartCoroutine(TypewriterEffect.PrintTypewriterText(typerwriterText, tutorialMessages[msgIndex], printDelay, skipKey));
                        msgIndex++;
                    }
                    else
                    {
                        if(playerController)
                        {
                            playerController.DisableInput = false;
                        }

                        StopSequence();
                    }
                }
            }
        }
    }

    public void StartSequence()
    {
        tutorialCanvas.SetActive(true);
        runningSequence = true;
    }

    public void StopSequence()
    {
        tutorialCanvas.SetActive(false);
        runningSequence = false;
    }
}