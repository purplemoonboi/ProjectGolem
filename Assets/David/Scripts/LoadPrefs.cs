using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MenuManager menuManager;

    [Header("Volume Setting")]
    [SerializeField] private TextMeshProUGUI volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;

    private void Awake()
    {
        if (canUse)
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("MasterVolume");
            }
        }
    }
}
