using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SoundData scriptable object
[CreateAssetMenu(fileName = "SoundData", menuName = "Sound Data")]
public class SoundData : ScriptableObject
{
    public List<AudioClip> sounds;
}