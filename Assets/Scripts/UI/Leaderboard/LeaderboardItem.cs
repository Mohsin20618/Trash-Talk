using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TrashTalk;
using System.IO;

public class LeaderboardItem : MonoBehaviour
{
    public Text indexText;
    public Text nameText;
    public Text wonText;
    public Text levelText;
    

    public Sprite Rank1Image;
    public Sprite Rank2Image;
    public Sprite Rank3Image;

    public Image indexImage;
    public Image thumb;
    public GameObject imageLoader;

    private string imageURL = "https://images.unsplash.com/photo-1558000959-d20934cc98ed?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1776&q=80";

    User user;

    public void SetData(int index, User user)
    {
        this.user = user;

        SetRank(index);
        nameText.text = user.FullName;
        wonText.text = user.winCount.ToString();
        levelText.text = Utility.CalculateUserLevel(user.winCount)[0].ToString();
        this.imageURL = user.ImagePath +"/"+ user.Image;
        if (!string.IsNullOrEmpty(this.imageURL))
        {
            Debug.Log("imageURL: " + this.imageURL);
            ImageCacheManager.instance.CheckOrDownloadImage(this.imageURL, this.thumb,DownloadCallBack);
        }
        else
            imageLoader.SetActive(false);
    }
    public void SetRank(int index) 
    { 
        if (index < 4)
        {
            if(index ==1)
                indexImage.sprite = Rank1Image;
            if(index == 2)
                indexImage.sprite = Rank2Image;
            if (index == 3)
                indexImage.sprite = Rank3Image;

            indexImage.gameObject.SetActive(true);
            indexText.gameObject.SetActive(false);
        }
        else
        {
            indexText.text = index.ToString();

            indexImage.gameObject.SetActive(false);
            indexText.gameObject.SetActive(true);
        }
    }
    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

}
