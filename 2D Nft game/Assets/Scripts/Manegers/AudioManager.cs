using UnityEngine;

// Serializable class to define properties of each sound
[System.Serializable]
public class Sound
{
    public string name; // Name of the sound
    public AudioClip clip; // Audio clip associated with the sound
    [Range(0f, 1f)]
    public float volume = 1f; // Volume of the sound
    [Range(0.1f, 3f)]
    public float pitch = 1f; // Pitch of the sound
    public bool loop; // Whether the sound should loop
    public bool isMusic; // Whether the sound is music
    public bool isSfx; // Whether the sound is a sound effect

    [HideInInspector]
    public AudioSource source; // AudioSource component associated with the sound
}

// AudioManager class to manage game sounds
public class AudioManager : MonoBehaviour
{
   

    public Sound[] sounds; // Array of sounds managed by the AudioManager

    [Range(0f, 1f)]
    public float masterVolume = 1f; // Master volume for all sounds
    [Range(0f, 1f)]
    public float musicVolume = 1f; // Volume for music sounds
    [Range(0f, 1f)]
    public float sfxVolume = 1f; // Volume for sound effects
    private bool isMusicMuted; // Flag to indicate if music is muted
    private bool isSfxMuted; // Flag to indicate if sound effects are muted

    // Awake method to initialize the singleton instance and set up sounds
    private void Awake()
    {


            // Create AudioSource components for each sound and set their properties
            foreach (Sound sound in sounds)
            {
                GameObject soundGameObject = new GameObject("AudioSource_" + sound.name);
                soundGameObject.transform.parent = transform;

                sound.source = soundGameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }

            // Load saved mute states and update volumes accordingly
            LoadMuteStates();
            UpdateAllVolumes();
       
    }

    // Start method to play the background sound at the beginning
    private void Start()
    {
       
    }

    // Play a sound by its name
    public void PlaySound(string name)
    {
        Sound sound = GetSoundByName(name);
        if (sound != null)
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    // Play a sound only if it is not already playing
    public void PlayOverSound(string name)
    {
        Sound sound = GetSoundByName(name);
        if (sound != null)
        {
            if (!sound.source.isPlaying)
            {
                sound.source.Play();
            }
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    // Stop playing a sound by its name
    public void StopSound(string name)
    {
        Sound sound = GetSoundByName(name);
        if (sound != null)
        {
            sound.source.Stop();
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    // Set the volume of a specific sound
    public void SetVolume(string name, float volume)
    {
        Sound sound = GetSoundByName(name);
        if (sound != null)
        {
            sound.source.volume = Mathf.Clamp01(volume);
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    // Set the pitch of a specific sound
    public void SetPitch(string name, float pitch)
    {
        Sound sound = GetSoundByName(name);
        if (sound != null)
        {
            sound.source.pitch = pitch;
        }
        else
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
        }
    }

    // Set the master volume and update all sound volumes
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    // Set the music volume and update music volumes
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        UpdateMusicVolume();
    }

    // Set the sound effects volume and update SFX volumes
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        UpdateSfxVolume();
    }

    // Update the master volume
    public void UpdateMasterVolume(float value)
    {
        SetMasterVolume(value);
    }

    // Mute the music and save the state
    public void MuteMusic()
    {
        isMusicMuted = true;
        SaveMuteStates();
        UpdateMusicVolume();
    }

    // Unmute the music and save the state
    public void UnmuteMusic()
    {
        isMusicMuted = false;
        SaveMuteStates();
        UpdateMusicVolume();
    }

    // Mute the sound effects and save the state
    public void MuteSfx()
    {
        isSfxMuted = true;
        SaveMuteStates();
        UpdateSfxVolume();
    }

    // Unmute the sound effects and save the state
    public void UnmuteSfx()
    {
        isSfxMuted = false;
        SaveMuteStates();
        UpdateSfxVolume();
    }

    // Mute all sounds and save the state
    public void MuteAll()
    {
        isMusicMuted = true;
        isSfxMuted = true;
        SaveMuteStates();
        UpdateAllVolumes();
    }

    // Unmute all sounds and save the state
    public void UnmuteAll()
    {
        isMusicMuted = false;
        isSfxMuted = false;
        SaveMuteStates();
        UpdateAllVolumes();
    }

    // Update the volume of all sounds based on their type and mute states
    private void UpdateAllVolumes()
    {
        foreach (Sound sound in sounds)
        {
            float soundVolume = sound.volume * masterVolume;

            if (sound.isMusic)
            {
                sound.source.volume = soundVolume * (isMusicMuted ? 0f : musicVolume);
            }
            else if (sound.isSfx)
            {
                sound.source.volume = soundVolume * (isSfxMuted ? 0f : sfxVolume);
            }
            else
            {
                sound.source.volume = soundVolume;
            }
        }
    }

    // Update the volume of all music sounds based on mute state
    private void UpdateMusicVolume()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.isMusic)
            {
                float soundVolume = sound.volume * masterVolume * (isMusicMuted ? 0f : musicVolume);
                sound.source.volume = soundVolume;
            }
        }
    }

    // Update the volume of all sound effects based on mute state
    private void UpdateSfxVolume()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.isSfx)
            {
                float soundVolume = sound.volume * masterVolume * (isSfxMuted ? 0f : sfxVolume);
                sound.source.volume = soundVolume;
            }
        }
    }

    // Get a sound by its name from the sounds array
    private Sound GetSoundByName(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                return sound;
            }
        }
        return null;
    }

    // Save the mute states using PlayerPrefs
    private void SaveMuteStates()
    {
        PlayerPrefs.SetInt("isMusicMuted", isMusicMuted ? 1 : 0);
        PlayerPrefs.SetInt("isSfxMuted", isSfxMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Load the mute states using PlayerPrefs
    private void LoadMuteStates()
    {
        isMusicMuted = PlayerPrefs.GetInt("isMusicMuted", 0) == 1;
        isSfxMuted = PlayerPrefs.GetInt("isSfxMuted", 0) == 1;
    }
}
