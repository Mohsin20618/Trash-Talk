using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    public static SoundManager Instance { get; private set; }

    private AudioSource backgroundMusicSource;
 //   private List<AudioSource> soundEffectSources;

    private Dictionary<string, AudioClip> soundEffect;

    public ButtonToggle musicToggle;
    bool isMusicOn = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
         //   soundEffectSources = new List<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
        soundEffect = new Dictionary<string, AudioClip>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        musicToggle.SetToggle(isMusicOn);
    }

    public void AddSound(string name, AudioClip clip)
    {
        soundEffect[name] = clip;
    }

    public void PlayBackgroundMusic(Sound soundName)
    {
        string name = soundName.ToString();

        SettingPanel.insideGamePlayScreen = true;

        if (soundEffect.ContainsKey(name))
        {
            AudioClip clip = soundEffect[name];
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();

        }
    }

    public void MuteBGSound()
    {
        isMusicOn = !isMusicOn;

        if (backgroundMusicSource)
            backgroundMusicSource.mute = !isMusicOn;
    }

    public void StopBackgroundMusic()
    {
        SettingPanel.insideGamePlayScreen = false;

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }
    }

    public void PlaySoundEffect(Sound soundName)
    {
        if (!isMusicOn)
            return;

        string name = soundName.ToString();
        if (soundEffect.ContainsKey(name))
        {
            AudioClip clip = soundEffect[name];
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.Play();
            StartCoroutine(CleanupSoundEffect(source));
        }


        //    soundEffectSources.Add(source);

        // Clean up finished sound effect sources
    }


    AudioSource audioSource;

    public void PlayTimerEffect()
    {
        if (!isMusicOn)
            return;

        string name = Sound.StopWatch.ToString();
        if (soundEffect.ContainsKey(name))
        {
            AudioClip clip = soundEffect[name];
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            
            StartCoroutine(_StopTimerEffect());
        }
    }

    public void StopTimerEffect()
    {
        StartCoroutine(_StopTimerEffect());
    }

    public IEnumerator _StopTimerEffect()
    {
        if (audioSource)
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            Destroy(audioSource);
        }
    }




    private IEnumerator CleanupSoundEffect(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(source);
    }
}
