using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashTalk;

public class LeaderboardItem : MonoBehaviour
{
    public Text indexText;
    public Text nameText;
    public Text wonText;

    public Image thumb;
    public GameObject imageLoader;

    private string imageURL = "https://images.unsplash.com/photo-1558000959-d20934cc98ed?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1776&q=80";

    User user;

    public void SetData(int index, User user)
    {
        this.user = user;

        indexText.text = $"{index}";
        nameText.text = this.user.FullName;
        wonText.text = $"Won {this.user.winCount}";

        if (this.imageURL != null && this.imageURL != "")
        {
            ImageCacheManager.instance.CheckOrDownloadImage(this.imageURL, this.thumb,DownloadCallBack);
        }
        else
            imageLoader.SetActive(false);
    }

    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

}
