using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSet[] audioSets;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SFXVolume";
    private const string MusicParam = "Music";
    private const string SfxParam = "SFX";
    private const float MinVolume = 0.0001f;
    private const float MaxVolume = 1f;

    private AudioSet currentAudioSet;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.ignoreListenerPause = true;
        sfxSource.ignoreListenerPause = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        HookSliderEvents();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        UnhookSliderEvents();
    }

    private void Start()
    {
        InitializeAudioLevels();
        LoadAudioForScene(SceneManager.GetActiveScene().name);
    }

    private void InitializeAudioLevels()
    {
        UpdateSliders();
        ApplyVolumeToMixer(GetSavedVolume(MusicVolumeKey), MusicParam);
        ApplyVolumeToMixer(GetSavedVolume(SfxVolumeKey), SfxParam);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadAudioForScene(scene.name);
    }

    private void LoadAudioForScene(string sceneName)
    {
        currentAudioSet = audioSets.FirstOrDefault(set => set.sceneName == sceneName);

        if (currentAudioSet?.music != null)
            PlayMusic(currentAudioSet.music);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || (musicSource.clip == clip && musicSource.isPlaying))
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(string name)
    {
        if (currentAudioSet?.sfxClips == null) return;

        AudioClip clip = currentAudioSet.sfxClips.FirstOrDefault(c => c.name == name);
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void OnMusicVolumeChanged(float value)
    {
        ApplyVolumeToMixer(value, MusicParam);
        SaveVolume(MusicVolumeKey, value);
    }

    public void OnSfxVolumeChanged(float value)
    {
        ApplyVolumeToMixer(value, SfxParam);
        SaveVolume(SfxVolumeKey, value);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void AssignSliders(Slider music, Slider sfx)
    {
        UnhookSliderEvents();

        musicSlider = music;
        sfxSlider = sfx;

        HookSliderEvents();
    }

    public void UpdateSliders()
    {
        if (musicSlider != null)
            musicSlider.value = GetSavedVolume(MusicVolumeKey);
        if (sfxSlider != null)
            sfxSlider.value = GetSavedVolume(SfxVolumeKey);
    }

    private void ApplyVolumeToMixer(float volume, string parameter)
    {
        float decibels = Mathf.Log10(Mathf.Max(volume, MinVolume)) * 20f;
        if (!audioMixer.SetFloat(parameter, decibels))
            Debug.LogWarning($"Mixer parameter '{parameter}' not found.");
    }

    private float GetSavedVolume(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            PlayerPrefs.SetFloat(key, MaxVolume);
        return PlayerPrefs.GetFloat(key);
    }

    private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    private void HookSliderEvents()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    private void UnhookSliderEvents()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
    }
}

[Serializable]
public class AudioSet
{
    public string sceneName;
    public AudioClip music;
    public AudioClip[] sfxClips;
}
