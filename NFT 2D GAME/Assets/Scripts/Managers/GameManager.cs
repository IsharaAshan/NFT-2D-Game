using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] AudioManager audioManager;

    public int MainLifeValue { get; set; }
    public int MainCoinValue { get; set; }

    [SerializeField] GameObject pausePanel;

    public Toggle muteToggle;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        MainLifeValue = 3;

       // PlayerPrefs.DeleteAll();

        MainCoinValue = PlayerPrefs.GetInt("maincoin", MainCoinValue); ;

        pausePanel?.SetActive(false);
    }

    private void Start()
    {
        // Attach event listeners
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        LoadSettings();
    }


    public void UpdateCoinValue(int value) 
    {
       
        PlayerPrefs.SetInt("maincoin",value); 
    }

    public void AddCoins(int value) 
    {
        MainCoinValue += value;

        UpdateCoinValue(MainCoinValue);
    }

    public void RemoveCoins(int value) 
    {
        if (value >= MainCoinValue) { MainCoinValue -= value; }
        else 
        {
            Debug.Log("Not enough coins");
        }

        if (MainCoinValue < 0) 
        {
            MainCoinValue = 0;
        }


        UpdateCoinValue(MainCoinValue);
    }

    #region //Audio

    public void PlaySfx(string clip)
    {
        audioManager.PlaySound(clip);
    }

    public void PausePanelOpen()
    {
        pausePanel.SetActive(true);
        PlaySfx("click");
        Time.timeScale = 0;
    }

    public void PausePanelClose()
    {
        pausePanel.SetActive(false);
        PlaySfx("click");
        Time.timeScale = 1;
    }

    public int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(GetActiveScene());
        PlaySfx("click");
    }

    public void LoadNextScene()
    {
        int maxScene = SceneManager.sceneCountInBuildSettings - 1;

        if (GetActiveScene() >= maxScene)
        {
            HomeMenu();
        }
        else
        {
            SceneManager.LoadScene(GetActiveScene() + 1);
        }

        PlaySfx("click");
    }

    public void HomeMenu()
    {
        SceneManager.LoadScene(0);
        PlaySfx("click");
    }



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

    void OnMusicVolumeChanged(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }
        SaveSettings();
    }

    void OnSFXVolumeChanged(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSfxVolume(volume);
        }
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Mute", muteToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Mute"))
        {
            muteToggle.isOn = PlayerPrefs.GetInt("Mute") == 1;
        }
        else
        {
            muteToggle.isOn = false;
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            musicVolumeSlider.value = 1f;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            sfxVolumeSlider.value = 1f;
        }

        ApplySettings();
    }

    void ApplySettings()
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(musicVolumeSlider.value);
            audioManager.SetSfxVolume(sfxVolumeSlider.value);

            if (muteToggle.isOn)
            {
                audioManager.MuteAll();
            }
            else
            {
                audioManager.UnmuteAll();
            }
        }
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

    #region Shop Values Change

    #endregion
}
