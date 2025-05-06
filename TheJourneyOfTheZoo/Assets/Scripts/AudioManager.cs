using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private MusicClip[] musicClips;
    [SerializeField] private MusicClip[] sfxClips;
    [SerializeField] private AudioMixer audioMixer;

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

    public void PlayMusic(string name)
    {
        MusicClip musicClip = Array.Find(musicClips, x => x.name == name);
        if (musicClip == null)
        {
            Debug.LogWarning($"Música {name} no encontrada!");
            return;
        }
        
        musicSource.clip = musicClip.clip;
        musicSource.Play();
    }

    public void PlaySfx(string name)
    {
        MusicClip sfxClip = Array.Find(sfxClips, x => x.name == name);
        if (sfxClip == null)
        {
            Debug.LogWarning($"Efecto de sonido {name} no encontrado!");
            return;
        }
        
        sfxSource.PlayOneShot(sfxClip.clip);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}

[Serializable]
public class MusicClip
{
    public string name;
    public AudioClip clip;
}