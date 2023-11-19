using UnityEngine;
using UnityEngine.Audio;
using LRS;

public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer audioMixer;
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup musicGroup;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    
    public string masterVolume = "MasterVolume";
    public string musicVolume = "MusicVolume";
    public string sfxVolume = "SFXVolume";

    [Header("Music")] 
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip[] gameMusic;
    
    [Header("SFX")]
    public AudioClip gameOverSound;
    public AudioClip floorClearedSound;

    [Header("UI")] 
    public AudioClip buttonHoverSound;
    public AudioClip buttonClickSound;

    protected override void Awake()
    {
       useDontDestroyOnLoad = true;
       base.Awake();
    }    
    
    public void PlaySound(AudioClip clip, AudioMixerGroup mixerGroup)
    {
        musicAudioSource.outputAudioMixerGroup = mixerGroup;
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxAudioSource.PlayOneShot(clip, volume);
    }
    
    public void PlayMusic(AudioClip clip)
    {
        PlaySound(clip, musicGroup);
    }

    public void PlayMusic()
    {
        if (musicAudioSource.clip != menuMusic)
        {
            PlaySound(menuMusic, musicGroup);
        }
        else if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolume, volume);
    }
    
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolume, volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolume, volume);
    }
    
    public void MuteAll()
    {
        audioMixer.SetFloat(masterVolume, -80);
    }

    public void MuteSfx()
    {
        audioMixer.SetFloat(sfxVolume, -80);
    }
    
    public void MuteMusic()
    {
        audioMixer.SetFloat(musicVolume, -80);
    }
}