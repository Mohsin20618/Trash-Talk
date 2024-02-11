using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour
{
    public GameObject selectedImg;
    public GameObject avatarImg;

    private void Start()
    {
        avatarImg.GetComponent<Button>().onClick.AddListener(OnSelectAvatar);
    }

    private void OnSelectAvatar()
    {
        AvatarUpdater.instance.UpdateAvatar(avatarImg.GetComponent<Image>().sprite.texture) ;
        EventManager.UpdateProfilePic?.Invoke(avatarImg.GetComponent<Image>().sprite);
    }

    public void ToggleSelection(bool select)
    {
        selectedImg.SetActive(select);
    }
}
