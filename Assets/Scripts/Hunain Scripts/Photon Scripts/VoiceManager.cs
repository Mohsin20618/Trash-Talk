using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviourPun
{

    public Recorder recorder;
    public Image micImage;


    public static VoiceManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void EnableDisableVoiceManager()
    {

        if (recorder != null && Global.isMultiplayer)
        {
            gameObject.SetActive(true);
            recorder.TransmitEnabled = true; // Enable voice transmission
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public void ResetMicIcon()
    {
        Color color = micImage.color;
        color.a = 1;
        micImage.color =  color;
    }

    public bool EnableDisableAudioTransmition()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }

        Color color = micImage.color;
        color.a = recorder.TransmitEnabled ? 1: .3f;
        micImage.color = color;
        return recorder.TransmitEnabled;
    }
}