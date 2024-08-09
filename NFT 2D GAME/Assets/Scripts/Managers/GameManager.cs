using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager which allows it to be accessed by any other script
    public static GameManager Instance { get; private set; }

    [SerializeField]AudioManager audioManager;
    // Awake is called when the script instance is being loaded

    public int MainLifeValue { get; set; }
    public int MainCoinValue { get; set; }

    [SerializeField] GameObject pausePanel;


    public UnityEvent<float> OnMoveSpeedChange;


    public Toggle muteToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        // Check if instance already exists and if it's not this, destroy the gameObject
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Make this the instance
            Instance = this;
            // Make sure this instance persists across scenes
            DontDestroyOnLoad(gameObject);
        }

        MainLifeValue = 3;

        pausePanel?.SetActive(false);
    }

   

    public void MoveSpeedChange(float value) 
    {
        OnMoveSpeedChange?.Invoke(value);
    }

    public void PlaySfx(string clip) 
    {
       audioManager.PlaySound(clip);
    }

    public void PausePanelOpen() 
    {
        pausePanel.SetActive(true);
        PlaySfx("click");
        Time.timeScale  = 0;
    }

    public void PausePanelClose() 
    {
        pausePanel.SetActive(false);
        PlaySfx("click");
        Time.timeScale = 1;
    }


    #region//Audio Settings
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

    public void PlayOverPlay(string soundName)
    {
        audioManager.PlaySound(soundName);
    }

    public void StopSound(string soundName)
    {
        audioManager.StopSound(soundName);
    }

    #endregion
}
