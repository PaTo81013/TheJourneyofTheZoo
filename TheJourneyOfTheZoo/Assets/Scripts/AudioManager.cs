using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSet[] audioSets;

    [FormerlySerializedAs("_musicVolumeMultiplier")] [Range(0, 1), SerializeField] private float musicVolumeMultiplier = 0.25f;
    [FormerlySerializedAs("_sfxVolumeMultiplier")] [Range(0, 1), SerializeField] private float sfxVolumeMultiplier = 0.7f;

    public const string MusicMixerVolumeParameterName = "Music";
    public const string SfxMixerVolumeParameterName = "SFX";

    public static float MusicVolumeValue => PlayerPrefs.GetFloat("MusicVolume", 1f);
    public static float SfxVolumeValue => PlayerPrefs.GetFloat("SFXVolume", 1f);

    private AudioSet _currentAudioSet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetMixerVolume(MusicVolumeValue, MusicMixerVolumeParameterName);
        SetMixerVolume(SfxVolumeValue, SfxMixerVolumeParameterName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadAudioForScene(scene.name);
    }

    private void LoadAudioForScene(string sceneName)
    {
        _currentAudioSet = audioSets.FirstOrDefault(set => set.sceneName == sceneName);

        if (_currentAudioSet == null)
        {
            Debug.LogWarning($"No se encontró configuración de audio para la escena {sceneName}");
            return;
        }

        PlayMusic(_currentAudioSet.music);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null)
        {
            Debug.LogWarning($"Clip de música nulo!");
            return;
        }

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return; // Ya está reproduciendo esta música

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void PlaySfx(string sfxName)
    {
        if (_currentAudioSet == null || _currentAudioSet.sfxClips == null)
        {
            Debug.LogWarning("No hay efectos de sonido disponibles para esta escena.");
            return;
        }

        AudioClip clip = _currentAudioSet.sfxClips.FirstOrDefault(c => c.name == sfxName);

        if (clip == null)
        {
            Debug.LogWarning($"Efecto de sonido {sfxName} no encontrado en esta escena.");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void SetMixerVolume(float volume, string mixerChannel)
    {
        const float minVolume = 0.0001f;

        switch (mixerChannel)
        {
            case MusicMixerVolumeParameterName:
                audioMixer.SetFloat(mixerChannel, Mathf.Log10((volume + minVolume) * musicVolumeMultiplier) * 20);
                PlayerPrefs.SetFloat("MusicVolume", volume);
                break;
            case SfxMixerVolumeParameterName:
                audioMixer.SetFloat(mixerChannel, Mathf.Log10((volume + minVolume) * sfxVolumeMultiplier) * 20);
                PlayerPrefs.SetFloat("SFXVolume", volume);
                break;
        }

        PlayerPrefs.Save();
    }
    public void OnMusicVolumeChanged(float value)
    {
        SetMixerVolume(value, MusicMixerVolumeParameterName);
    }

    public void OnSfxVolumeChanged(float value)
    {
        SetMixerVolume(value, SfxMixerVolumeParameterName);
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }
}

[Serializable]
public class AudioSet
{
    public string sceneName;
    public AudioClip music;
    public AudioClip[] sfxClips;
}
