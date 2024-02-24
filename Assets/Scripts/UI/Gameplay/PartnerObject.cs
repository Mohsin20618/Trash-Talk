using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PartnerObject : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Text gameScore;
    public Image profileImage;
    public GameObject imageLoader;

    string userId;

    public void SetData(string name = "", string userId = "", string imageUrl = null)
    {
        this.userId = userId;

        nameText.text = name;
     //   gameScore.text = score.ToString();

        if (profileImage != null)
        {
            if (imageUrl != null && imageUrl != "")
            {
                ImageCacheManager.instance.CheckOrDownloadImage(imageUrl, this.profileImage, DownloadCallBack);
            }
            else
                imageLoader.SetActive(false);
        }
    }

    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

    public void OnClick()
    {
        print("Selected Partner");
        List<string> temp = new();
        List<string> opponents = GetOpponents();

        temp.Add(PhotonNetwork.LocalPlayer.UserId);
        temp.Add(opponents.Count > 0 ? opponents[0] : "");
        temp.Add(this.userId);
        temp.Add(opponents.Count > 1 ? opponents[1] : "");

        PhotonRPCManager.Instance.SendMaterList(temp);

        GameplayManager.instance.partnerPanel.EnableDisablePanel(false);


    }

    List<string> GetOpponents() 
    {
        List<string> players = new();

        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.UserId != PhotonNetwork.LocalPlayer.UserId && item.UserId != this.userId)
            {
                players.Add(item.UserId);
            }

        }

        return players;
    }
}
