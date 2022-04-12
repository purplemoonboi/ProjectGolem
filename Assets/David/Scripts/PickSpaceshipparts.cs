using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickSpaceshipparts : MonoBehaviour
{

    public GameObject popUpMessage;
    public GameObject objectToPickUp;

    public int countParts;
    public Image piecesImg;

    public Sprite missingPieceSprite;
    public GameObject pickupText;

    bool isOpened;

    // Start is called before the first frame update
    void Start()
    {
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (popUpMessage == null || objectToPickUp == null || piecesImg == null || missingPieceSprite == null)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            pickupText.SetActive(false);
            popUpMessage.SetActive(true);
            isOpened = true;
            countParts++;
            piecesImg.sprite = missingPieceSprite;
            Time.timeScale = 0f;
        }
        if (isOpened)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                piecesImg.sprite = null;
                popUpMessage.SetActive(false);
                objectToPickUp.SetActive(false);
                Time.timeScale = 1f;
            }
        }  
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pickupText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pickupText.SetActive(false);
    }

}
