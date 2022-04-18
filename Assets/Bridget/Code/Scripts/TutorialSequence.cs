using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField] private Text typerwriterText;
    [SerializeField] private Text skipMessageText;

    [SerializeField] private List<string> tutorialMessages;

    [SerializeField] private KeyCode skipKey;

    [SerializeField] private int index = 0;
    [SerializeField] private bool runningSequence = true;

    void Start()
    {
        skipMessageText.text = "'" + skipKey.ToString() + "' TO SKIP";
        tutorialMessages = new List<string>();
        tutorialMessages.Add("hello, this is another test message! blah blah blah blabhdhdhbdh blah blah!!!1!");
        tutorialMessages.Add("This is another message! woooooo32krkewkf");
        tutorialMessages.Add("This is another message AGAIN! NUMBER 3 woooooo32krkewkf");
        tutorialMessages.Add("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA 4");
    }

    void Update()
    {
        //Updating the skip/continue text
        if(TypewriterEffect.isPrinting)
            skipMessageText.text = "'" + skipKey.ToString() + "' TO SKIP";
        else
            skipMessageText.text = "'" + skipKey.ToString() + "' TO CONTINUE";

        //Checking if the skip/continue button has been pressed and deciding what to print accordingly
        if (Input.GetKeyDown(skipKey) || index == 0)
        {
            if(TypewriterEffect.isPrinting)
            {
                TypewriterEffect.skippedDialogue = true;
            }

            else
            {
                if (index < tutorialMessages.Count)
                {
                    StartCoroutine(TypewriterEffect.PrintTypewriterText(typerwriterText, tutorialMessages[index], 0.033f, skipKey));
                    index++;
                }
                else
                    runningSequence = false;
            }
        }
    }
}