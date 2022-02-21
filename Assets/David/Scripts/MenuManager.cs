/*Music from Uppbeat (free for Creators!):
https://uppbeat.io/t/soundroll/sounds-from-space
License code: GJV1YDMHWIPSCEPK*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//@author David Costa

public class MenuManager : MonoBehaviour
{

    [Header("Volume Settings")]
    [SerializeField] private TextMeshProUGUI masterVolumeValue = null;
    [SerializeField] private Slider masterVolumeSlider = null;
    [SerializeField] private TextMeshProUGUI backgroundVolumeValue = null;
    [SerializeField] private Slider backgroundVolumeSlider = null;
    [SerializeField] private TextMeshProUGUI sfxVolumeValue = null;
    [SerializeField] private Slider sfxVolumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;
    [SerializeField] private float defaultBackgroundVolume = 0.5f;
    [SerializeField] private float sfxDefaultVolume = 0.5f;

    public AudioSource backgroundAudio;
    public AudioSource[] soundEffectsAudio;

    [SerializeField] private GameObject confirmationPrompt = null;

    private static readonly string firstPlay = "FirstPlay";
    private static readonly string masterPref = "MasterVolume";
    private static readonly string backgroundPref = "BackgroundVolume";
    private static readonly string sfxPref = "SfxVolume";
    private int firstPlayInt;
    private float masterFloat;
    private float backgroundFloat;
    private float sfxFloat;


    [Header("Graphics Settings")]
    private int qualityLevel;
    private bool isFullscreen;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;




    private void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(firstPlay);
        
        if (firstPlayInt == 0)
        {
            masterFloat = 1.0f;
            backgroundFloat = 0.5f;
            sfxFloat = 0.5f;
            masterVolumeSlider.value = masterFloat;
            backgroundVolumeSlider.value = backgroundFloat;
            sfxVolumeSlider.value = sfxFloat;
            PlayerPrefs.SetFloat(masterPref, masterFloat);
            PlayerPrefs.SetFloat(backgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(sfxPref, sfxFloat);
            PlayerPrefs.SetInt(firstPlay, -1);
        }
        else
        {
            masterFloat =  PlayerPrefs.GetFloat(masterPref);
            masterVolumeSlider.value = masterFloat;
            backgroundFloat = PlayerPrefs.GetFloat(backgroundPref);
            backgroundVolumeSlider.value = backgroundFloat;
            sfxFloat =  PlayerPrefs.GetFloat(sfxPref);
            sfxVolumeSlider.value = sfxFloat;
        }

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(masterPref, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(backgroundPref, backgroundVolumeSlider.value);
        PlayerPrefs.SetFloat(sfxPref, sfxVolumeSlider.value);
    }

    public void SetMasterVolume(float masterVolume)
    {
        AudioListener.volume = masterVolume;
        masterVolumeValue.text = masterVolume.ToString("0.0");
    }

    public void SetBackgroundVolume(float backgroundVolume)
    {
        backgroundAudio.volume = backgroundVolume;
        backgroundVolumeValue.text = backgroundVolume.ToString("0.0");
    }

    public void SetSfxVolume(float sfxVolume)
    {
        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = sfxVolumeSlider.value;
        }
        sfxVolumeValue.text = sfxVolume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat(masterPref, defaultVolume);
        PlayerPrefs.SetFloat(backgroundPref, defaultBackgroundVolume);
        PlayerPrefs.SetFloat(sfxPref, sfxDefaultVolume);
        StartCoroutine(ConfirmationBox());
    }

    //public void UpdateSound()
    //{
    //    backgroundAudio.volume = backgroundVolumeSlider.value;
    //    for (int i = 0; i < soundEffectsAudio.Length; i++)
    //    {
    //        soundEffectsAudio[i].volume = sfxVolumeSlider.value;
    //    }
    //}

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void ResetButton(string menuType)
    {
        if (menuType == "MasterAudio")
        {
            AudioListener.volume = defaultVolume;
            masterVolumeSlider.value = defaultVolume;
            masterVolumeValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        else if (menuType == "BackgroundAudio")
        {
            AudioListener.volume = defaultBackgroundVolume;
            backgroundVolumeSlider.value = defaultBackgroundVolume;
            backgroundVolumeValue.text = defaultBackgroundVolume.ToString("0.0");
            VolumeApply();
        }
        else if (menuType == "SfxAudio")
        {
            AudioListener.volume = sfxDefaultVolume;
            sfxVolumeSlider.value = sfxDefaultVolume;
            sfxVolumeValue.text = sfxDefaultVolume.ToString("0.0");
            VolumeApply();
        }
        else if (menuType == "Graphics")
        {
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }
    }

    public void SetFullscreen(bool fullScreen)
    {
        isFullscreen = fullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;

        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
