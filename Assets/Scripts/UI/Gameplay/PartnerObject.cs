using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PartnerObject : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Text gameScore;
    public Image profileImage;
    public GameObject imageLoader;

    System.Action<string> onSelect;
    string userId;

    public void SetData(System.Action<string> onSelect, string name = "", string userId = "", string imageUrl = null)
    {
        this.userId = userId;
        this.onSelect = onSelect;

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
      //  print("Selected Partner");
     //   this.onSelect(userId);
    }


}
