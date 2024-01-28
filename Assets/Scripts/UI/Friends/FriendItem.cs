using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TrashTalk;

public class FriendItem : MonoBehaviour
{
    public GameObject addButton;
    public GameObject inviteButton;
    private Transform selectionBtton;

    public Text nameText;
    public Text wonText;

    public Image thumb;
    public GameObject imageLoader;

    private string imageURL = "";
    bool isSelected = false;

    Action<User, bool> selectionCallBack;

    User user;

    public void SetData(int type, User user, Action<User, bool> selectCallBack)
    {
        this.user = user;
        this.selectionCallBack = selectCallBack;
        this.imageURL = user.ImagePath + "/" + user.Image;
        addButton.SetActive(type == 0 || type==1);
        inviteButton.SetActive(type == 2);
        selectionBtton = (type == 0 || type == 1) ? addButton.transform: inviteButton.transform;
        selectionBtton.gameObject.SetActive(user.UserId != PlayerProfile.Player_UserID);
        nameText.text = this.user.FullName;
        wonText.text = $"Won {this.user.winCount} matches" ;


        if (this.imageURL != null && this.imageURL != "")
        {
            ImageCacheManager.instance.CheckOrDownloadImage(this.imageURL, this.thumb, DownloadCallBack);
        }
        else
            imageLoader.SetActive(false);
    }


    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

    public void SelectUnselect()
    {
        this.selectionCallBack(this.user, isSelected = !isSelected);
        selectionBtton.GetChild(0).gameObject.SetActive(!isSelected);
        selectionBtton.GetChild(1).gameObject.SetActive(isSelected);

    }
}
