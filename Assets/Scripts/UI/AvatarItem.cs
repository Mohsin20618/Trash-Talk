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
        Debug.Log("OnSelectAvatar: " + avatarImg.GetComponent<Image>().sprite.name);
    }

    public void ToggleSelection(bool select)
    {
        selectedImg.SetActive(select);
    }
}
