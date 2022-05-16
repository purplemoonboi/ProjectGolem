using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateMenuImage : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Sprite sprite;

    //Update panel image;
    public void Cursor()
    {
        panel.GetComponent<Image>().sprite = sprite;
    }
}
