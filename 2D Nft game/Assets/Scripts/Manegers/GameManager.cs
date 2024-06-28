using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Reference to the AudioManager
    public AudioManager audioManager;

    // UI elements for controlling audio settings
    public Toggle muteToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
   // public Button SettingsButton;
   // public Button RestartButton;
  //  public Button HomeButton;
   // public Button ResumeButton;
    public Button settingsPanelCloseButton;

    public GameObject settingsPanel;



    private void Awake()
    {
        // Check if there is already an instance of GameManager
        if (Instance == null)
        {
            // If not, set it to this instance and make sure it persists between scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        // Add listeners to the UI elements
        if (muteToggle != null)
        {
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        settingsPanelCloseButton.onClick.AddListener(CloseSettingsPanel);

        LoadSettings();

        settingsPanel.SetActive(false);
    }

    // Callback for mute toggle
    void OnMuteToggleChanged(bool isMuted)
    {
        if (audioManager != null)
        {
            if (isMuted)
            {
                audioManager.MuteAll();
            }
            else
            {
                audioManager.UnmuteAll();
            }
        }
        SaveSettings();
    }

    // Callback for music volume slider
    void OnMusicVolumeChanged(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }
        SaveSettings();
    }

    // Callback for SFX volume slider
    void OnSFXVolumeChanged(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSfxVolume(volume);
        }
        SaveSettings();
    }

    // Save the audio settings
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Mute", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();

    }

    // Load the audio settings
    public void LoadSettings()
    {
        // Load or set default mute setting
        if (PlayerPrefs.HasKey("Mute"))
        {
            muteToggle.isOn = PlayerPrefs.GetInt("Mute") == 1;
        }
        else
        {
            muteToggle.isOn = false; // Default value for first time (unmuted)
        }

        // Load or set default music volume
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicVolumeSlider.value = 1f; // Default value for first time
        }

        // Load or set default SFX volume
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            sfxVolumeSlider.value = 1f; // Default value for first time
        }

        // Apply loaded or default settings
        ApplySettings();
    }


    // Apply settings to AudioManager
    void ApplySettings()
    {
        OnMuteToggleChanged(muteToggle.isOn);
        OnMusicVolumeChanged(musicVolumeSlider.value);
        OnSFXVolumeChanged(sfxVolumeSlider.value);
    }

    public void PlaySound(string soundName)
    {
        audioManager.PlaySound(soundName);
    }

    public void PlayOverPlay(string soundName)
    {
        audioManager.PlaySound(soundName);
    }

    public void StopSound(string soundName)
    {
        audioManager.StopSound(soundName);
    }

    // Your game management code goes here
    public void StartGame()
    {
        // Code to start the game
        Debug.Log("Game Started");
    }

    public void EndGame()
    {
        // Code to end the game
        Debug.Log("Game Ended");
    }

    public void RestartGame()
    {
        // Code to restart the game
        Debug.Log("Game Restarted");
    }

    public void OpenSettingsPanel()
    {
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
        PlaySound("Click");
    }

    public void CloseSettingsPanel()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
        PlaySound("Click");
    }

    public int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
