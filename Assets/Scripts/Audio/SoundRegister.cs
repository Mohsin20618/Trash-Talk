using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRegister : MonoBehaviour
{
    public SoundData soundData;

    void Start()
    {
        foreach (AudioClip sound in soundData.sounds)
        {
            SoundManager.Instance.AddSound(sound.name,sound);
        }
    }
}