using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class AvatarUpdater : MonoBehaviour
{
    public int maxSize = 256;
    public Transform content;
    public GameObject avatarRow;
    public GameObject avatarPickItem;
    public GameObject avatarItem;
    public List<Sprite> avatarList;
    public List<AvatarItem> avatarItemsList;

    public int itemsPerRow = 4;
    public int numRows; 

    public Texture2D texture = null;
    internal static AvatarUpdater instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        numRows = ((int)Math.Ceiling((double)avatarList.Count + 1) / itemsPerRow);

        for (int i = 0; i < numRows; i++)
        {
            var row = Instantiate(avatarRow, content);
            for (int j = 0; j < itemsPerRow; j++)
            {
                int index = i * itemsPerRow + j;
                if (index < avatarList.Count + 1)
                {
                    if (index == 0)
                    {
                        var ai = Instantiate(avatarPickItem, row.transform);
                        ai.GetComponentInChildren<Button>().onClick.AddListener(PickImage);
                    }
                    else
                    {
                        var avatar = Instantiate(avatarItem, row.transform);
                        AvatarItem ai = avatar.GetComponent<AvatarItem>();
                        ai.avatarImg.GetComponent<Image>().sprite = avatarList[index - 1];
                        avatarItemsList.Add(ai);
                    }
                }
            }
        }
    }


    public void PickImage()
    {
        Debug.Log("Image path: ");
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Debug.Log("Image path: " + path);
                // Create Texture from selected image
                texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                Sprite texSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                EventManager.UpdateProfilePic?.Invoke(texSprite);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    MesgBar.instance.show("Couldn't load Image", true);
                    return;
                }
            }
        }, "Select a JPG image", "image/jpg");
        if (permission == NativeGallery.Permission.Denied)
        {
            MesgBar.instance.show("Permission Denied! Allow Permission From Phone Setting", true);

        }
        Debug.Log("Permission result: " + permission);
    }


    public void UpdateAvatar(Texture2D texture2d)
    {
        //texture = texture2d;

        foreach (AvatarItem item in avatarItemsList)
        {
            print("HELLO : " + texture2d.name + "==" + item.avatarImg.GetComponent<Image>().sprite.texture.name);
            item.selectedImg.SetActive(texture2d.name == item.avatarImg.GetComponent<Image>().sprite.texture.name);
        }

        // TEMPERORY
        
    }
}
