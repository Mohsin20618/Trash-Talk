using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundManager
{
    void PlayBackgroundMusic(Sound name);
    void PlaySoundEffect(Sound name);
    void StopBackgroundMusic();
    void AddSound(string name, AudioClip clip);
}
