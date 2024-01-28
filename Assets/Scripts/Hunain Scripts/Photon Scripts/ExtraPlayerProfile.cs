using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ExtraPlayerProfile : MonoBehaviourPunCallbacks
{
    [Header("Texts")]
    public Text Chips;
    public Text PlayerName;

    [Header("Picture")]
    public RawImage rawImage;

    private void Start()
    {
        Debug.LogError("+++++++++++++++++++++++++="+gameObject.name);
        Chips.text = PlayerProfile.Player_coins.ToString();
        PlayerName.text = PlayerProfile.Player_UserName;
        rawImage.texture = PlayerProfile.Player_rawImage_Texture2D;
    }

    public new void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        int newscore = ScoreExtensions.GetScore(targetPlayer);
        if (targetPlayer.UserId == PlayerProfile.Player_UserID)
        {
            Chips.text = newscore.ToString();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
