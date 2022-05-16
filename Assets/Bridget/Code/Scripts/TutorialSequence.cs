using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private Text typerwriterText;
    [SerializeField] private Text skipMessageText;
    [SerializeField] private GameObject NPC;
    [SerializeField] private List<string> tutorialMessages = new List<string>();
    [SerializeField] private float printDelay = 0.033f;
    [SerializeField] private KeyCode skipKey;
    [SerializeField] private int msgIndex = 0;
    [SerializeField] private bool runningSequence = false;


    [SerializeField] private Collider[] colliders;
    [SerializeField] private string buildingType;

    private ThirdPersonController playerController;
    private Interact playerInteract;
    private bool disableFirstTimeCollider = false;
    private bool isColliderCooldown = false;
    private float colliderCooldown = 0f;

    private void Start()
    {
        colliders = new Collider[2];
        colliders = GetComponents<Collider>();
        tutorialCanvas.SetActive(false);
    }

    void Update()
    {
        if(isColliderCooldown == true)
        {
            colliderCooldown += Time.deltaTime;

            if(colliderCooldown >= 3f)
            {
                isColliderCooldown = false;
                colliderCooldown = 0f;
                colliders[1].enabled = true;
            }
        }
        else
        {
            if (runningSequence)
            {
                playerInteract.SetIsTalking(true);
                //Update NPC rotation
                //Vector3 lookAt = (playerController.transform.position - NPC.transform.position).normalized;
                //Quaternion rotationGoal = Quaternion.LookRotation(lookAt);
                //NPC.transform.rotation = Quaternion.Lerp(NPC.transform.rotation, rotationGoal, 2f * Time.deltaTime);

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
                            if (playerController)
                            {
                                playerController.DisableInput = false;
                            }

                            StopSequence();
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
                playerController = other.gameObject.GetComponent<ThirdPersonController>();
                playerInteract = other.gameObject.GetComponent<Interact>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerController && playerInteract)
            {
                if (playerInteract.IsInteractKey())
                {
                    if (!disableFirstTimeCollider)
                    {
                        colliders[0].enabled = false;
                        disableFirstTimeCollider = true;
                    }
                    playerController.DisableInput = true;
                    playerController.SetLookAtTarget(NPC.transform);
                    StartSequence();
                }
            }
        }
    }

    public void StartSequence()
    {
        playerInteract.SetIsTalking(true);
        tutorialCanvas.SetActive(true);
        runningSequence = true;
    }

    public void StopSequence()
    {
        playerInteract.SetIsTalking(false);
        tutorialCanvas.SetActive(false);
        runningSequence = false;
        msgIndex = 0;
        isColliderCooldown = true;
        colliders[1].enabled = false;
        //If the player has already met the NPC.
        if(disableFirstTimeCollider)
        {
            if(buildingType != "Resource")
            {
                string[] messages =
                {
                   "Welcome Back!",
                   "Remember this is the " + buildingType + " building.",
                   "Upgrading this building improves the performance of " +
                   (buildingType == "Weapons" ? " friendly turrets and fighters damage output." :
                   (buildingType == "Camp"    ? " your overall bases' health and damage dealt to enemies." : " your engineers' repair rate and health.")),
                   "Comeback anytime to upgrade when you have the resources!"
                };

                tutorialMessages.Clear();
                foreach (string s in messages)
                {
                    tutorialMessages.Add(s);
                }
            }
            else
            {
                string[] messages =
                {
                   "Remember, you can BOOST by pressing the 'SPACE' key!",
                   "You can farm resources by 'HOLDING down the SPACE Key'...",
                   "Resources are hard to miss... They aren't like the rest of these rocks...",
                   "Best head to the base just ahead before nightfall... Build some defences too!",
                };

                tutorialMessages.Clear();
                foreach (string s in messages)
                {
                    tutorialMessages.Add(s);
                }
            }

        }

    }
}