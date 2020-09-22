/*The purpose of this script is to allow us to use an options menu in game
This will be active on the menu or in game
It will allow us to adjust the settings and save them*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour 
{
    //Custom AudioMixer for adjusting the volume
	public AudioMixer audioMixer;
    public AudioManager audioManager;

    public static SettingsMenu instance;
    public Animator anim;

    //An array of resolutions for the player to choose from
    //PC ONLY
    Resolution[] resolutions;

    private int currentResolutionIndex = 0;

    public Text resolutionText;

    //This is the dropdown box that allows the user to select a resolution from the resolutions array
    public Dropdown resolutionDropdown;

    public Slider volumeSlider;
    public Toggle musicToggle;
    public Toggle fullscreenToggle;

    //PlayerPrefs
    string volumeKey = "GameVolume";
    //Default value of the volume is 0 in the volume settings
    float volumeValue;
    string fullscreenKey = "FullScreenKey";
    public GameObject creditsMenu;

    void Awake()
    {
        volumeValue = GetVolume();
        volumeSlider.value = volumeValue;;
        GetVolume();
        HideCredits();
    }

    void Start()
	{
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";

            if (options.Contains(option) == false)
            {
                options.Add(option);
            }

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == PlayerPrefs.GetInt("RefreshRate"))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        if(currentResolutionIndex != GetResolution())
        {
            resolutionDropdown.value = GetResolution();
        }
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
        }

        resolutionDropdown.RefreshShownValue();

        Screen.SetResolution(PlayerPrefs.GetInt("Resolution X"), PlayerPrefs.GetInt("Resolution Y"), Screen.fullScreen, PlayerPrefs.GetInt("RefreshRate"));
        resolutionText.text = PlayerPrefs.GetInt("Resolution X").ToString() + " x " + PlayerPrefs.GetInt("Resolution Y").ToString() + " " + PlayerPrefs.GetInt("RefreshRate").ToString() + "Hz";

        audioMixer.SetFloat("volume", volumeValue);

        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        volumeValue = GetVolume();
        volumeSlider.value = volumeValue;
        GetVolume();
        fullscreenToggle.isOn = GetFullScreen();
        SetFullScreen(GetFullScreen());
        Screen.SetResolution(PlayerPrefs.GetInt("Resolution X"), PlayerPrefs.GetInt("Resolution Y"), Screen.fullScreen, PlayerPrefs.GetInt("RefreshRate"));

    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeKey, volumeValue);
    }
	
    //Each Set method is use to set whichever variable it controls

    //Sets the volume by taking in a float for the volume
    //and setting the Audio Mixers volume to the specified amount
    //from the Volume slider
	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("volume", volume);
        volumeValue = volume;
        PlayerData.VolumeLevel = volume;
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(volumeKey);
        
    }
	
    //This method sets the quality based on the quality setting that corrosponds to the int passed in
	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}
	
    //Sets the fullscreen toggle in the Screen Class
    
	public void SetFullScreen(bool isFullScreen)
	{
        PlayerPrefs.SetInt(fullscreenKey, isFullScreen ? 1 : 0);
        Screen.fullScreen = isFullScreen;
        Resolution resolution = Screen.currentResolution;
        Screen.SetResolution(PlayerPrefs.GetInt("Resolution X"), PlayerPrefs.GetInt("Resolution Y"), isFullScreen);
        PlayerPrefs.SetInt("RefreshRate", resolution.refreshRate);
    }
    
	
    //Takes the resolutions and adjusts based on the selected resolution
    //Creates a temp resolution and assigns the current resolutions to its value
    
	public void SetResolution(int resolutionIndex)
	{
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        PlayerPrefs.SetInt("Resolution X", resolutions[resolutionIndex].width);
        PlayerPrefs.SetInt("Resolution Y", resolutions[resolutionIndex].height);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.SetInt("RefreshRate", resolutions[resolutionIndex].refreshRate);
    }

    public int GetResolution()
    {
        return PlayerPrefs.GetInt("ResolutionIndex");
    }

    public void ToggleAudio(bool audioEnabled)
    {
        audioManager.audioEnabled = audioEnabled;
        audioManager.EnableAudio();
    }

    //Find the Audio Manager when this object is enabled
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        musicToggle.isOn = audioManager.audioEnabled;
        SetFullScreen(GetFullScreen());
        Screen.SetResolution(PlayerPrefs.GetInt("Resolution X"), PlayerPrefs.GetInt("Resolution Y"), Screen.fullScreen, PlayerPrefs.GetInt("RefreshRate"));
    }

    public bool GetFullScreen()
    {
        return PlayerPrefs.GetInt(fullscreenKey) == 1 ? true : false;
    }

    public void DisplayCredits()
    {
        creditsMenu.SetActive(true);
    }

    public void HideCredits()
    {
        creditsMenu.SetActive(false);
    }

}