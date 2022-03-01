using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{

    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Text infoText;


    // Start is called before the first frame update
    void Start()
    {
        infoText = canvas.GetComponentInChildren<Text>();
        infoText.enabled = false;
        Image image = canvas.GetComponentInChildren<Image>();
        image.enabled = false;
    }

    public void DisableInfoPanel()
    {
        Image image = canvas.GetComponentInChildren<Image>();
        infoText.enabled = false;
        image.enabled = false;
    }

    public void EnableInfoPanel()
    {
        Image image = canvas.GetComponentInChildren<Image>();
        infoText.enabled = true;
        image.enabled = true;
    }

    public void SetText(Text text) => infoText.text = text.text;

    public Text GetText() => infoText;
}
