using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSet[] audioSets;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public const string MusicMixerVolumeParameterName = "Music";
    public const string SfxMixerVolumeParameterName = "SFX";

    private const float MinVolume = 0.0001f;
    private const float MaxVolume = 1f;

    public static float MusicVolumeValue
    {
        get
        {
            if (!PlayerPrefs.HasKey("MusicVolume"))
                PlayerPrefs.SetFloat("MusicVolume", MaxVolume);
            return PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    public static float SfxVolumeValue
    {
        get
        {
            if (!PlayerPrefs.HasKey("SFXVolume"))
                PlayerPrefs.SetFloat("SFXVolume", MaxVolume);
            return PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    private AudioSet _currentAudioSet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource.ignoreListenerPause = true;
            sfxSource.ignoreListenerPause = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
    }

    private void Start()
    {
        InitializeAudioLevels();
        LoadAudioForScene(SceneManager.GetActiveScene().name); // Cargar audio de la escena actual al iniciar
    }

    private void InitializeAudioLevels()
    {
        UpdateSliders();

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
            Debug.LogWarning("Clip de música nulo!");
            return;
        }

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

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

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip de SFX es nulo.");
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void SetMixerVolume(float volume, string mixerChannel)
    {
        float decibels = Mathf.Log10(Mathf.Max(volume, MinVolume)) * 20f;

        bool result = audioMixer.SetFloat(mixerChannel, decibels);
        if (!result)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"No se encontró el parámetro '{mixerChannel}' en el AudioMixer.");
#endif
        }

        switch (mixerChannel)
        {
            case MusicMixerVolumeParameterName:
                PlayerPrefs.SetFloat("MusicVolume", volume);
                break;
            case SfxMixerVolumeParameterName:
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
    
    public void AssignSliders(Slider music, Slider sfx)
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);

        musicSlider = music;
        sfxSlider = sfx;

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    public void UpdateSliders()
    {
        if (musicSlider != null)
            musicSlider.value = MusicVolumeValue;
        if (sfxSlider != null)
            sfxSlider.value = SfxVolumeValue;
    }
}

[Serializable]
public class AudioSet
{
    public string sceneName;
    public AudioClip music;
    public AudioClip[] sfxClips;
}
