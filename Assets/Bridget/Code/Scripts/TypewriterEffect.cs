using System.Collections;
using UnityEngine.UI;
using UnityEngine;

//Responsible for breaking up a string passed in into a character array and printing them to the screen in increments
public class TypewriterEffect : MonoBehaviour
{
    public static bool isPrinting = false;
    public static bool skippedDialogue = false;

    public static IEnumerator PrintTypewriterText(Text textComponent, string message, float delay, KeyCode skipKey)
    {
        isPrinting = true;

        textComponent.text = "";
        int i = 0;

        for(i = 0; i < message.Length; i++)
        {
            if (skippedDialogue)
            {
                textComponent.text += message.Substring(i);
                break;
            }

            textComponent.text += message[i];
            yield return new WaitForSeconds(delay);
        }

        skippedDialogue = false;
        isPrinting = false;
    }
}