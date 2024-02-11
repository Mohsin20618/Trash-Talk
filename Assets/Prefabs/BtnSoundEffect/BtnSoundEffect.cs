using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSoundEffect : MonoBehaviour
{
    public static BtnSoundEffect instance;
    public AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }
   public void PlayBtnSoundEffect() 
    {
        audioSource.Play();
    }

}
