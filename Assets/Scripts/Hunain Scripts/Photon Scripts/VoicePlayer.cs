using Photon.Voice.Unity;
using UnityEngine;


public class VoicePlayer : MonoBehaviour
{
    public bool isLocalPlayer = false;
    public string userId = "";

    public Photon.Realtime.Player photonPlayer;

    public AudioSource audioSource;
    public Speaker speaker;

    public void SetData(Photon.Realtime.Player player)
    {
        isLocalPlayer = player.IsLocal;
        userId = player.UserId;
        photonPlayer = player;
        //EnableDisableAudioSource(!isLocalPlayer);
    }

    public void EnableDisableAudioSource()
    {
        audioSource.enabled = speaker.enabled = !audioSource.enabled;
    }
}