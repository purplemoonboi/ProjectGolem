using UnityEngine;
using UnityEngine.UI;
using TMPro;

//@author David Costa

public class LoadPrefs : MonoBehaviour
{
    //[Header("General Setting")]
    //[SerializeField] private bool canUse = false;
    //[SerializeField] private MenuManager menuManager;

    [Header("Volume Setting")]
    //[SerializeField] private TextMeshProUGUI volumeTextValue = null;
    //[SerializeField] private Slider volumeSlider = null;
    private static readonly string masterPref = "MasterVolume";
    private static readonly string backgroundPref = "BackgroundVolume";
    private static readonly string sfxPref = "SfxVolume";
    private float masterFloat;
    private float backgroundFloat;
    private float sfxFloat;

    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    //[Header("Quality Level Setting")]
    //[SerializeField] private TMP_Dropdown qualityDropdown;

    //[Header("Fullscreen Setting")]
    //[SerializeField] private Toggle fullScreenToggle;

    private void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        masterFloat = PlayerPrefs.GetFloat(masterPref);
        backgroundFloat = PlayerPrefs.GetFloat(backgroundPref);
        sfxFloat = PlayerPrefs.GetFloat(sfxPref);
        
        
        backgroundAudio.volume = backgroundFloat;

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = sfxFloat;
        }

        //

    }
}
