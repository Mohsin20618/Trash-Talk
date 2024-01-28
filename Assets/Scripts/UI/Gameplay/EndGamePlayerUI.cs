using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndGamePlayerUI : MonoBehaviour
{
    public Image profileImage;
    public GameObject imageLoader;

    public Text nameText;
    public Text gameScore;

    public GameObject crown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUI(string name = "Waiting...", Sprite botSprite = null, int score = 0, string imageUrl = null, bool isWinner=false)
    {
        crown.SetActive(isWinner);

        if (nameText != null)
            nameText.text = name;
        if (gameScore != null)
            gameScore.text = score.ToString();


        if (profileImage != null)
        {
            //Profile work
            if (botSprite != null)
            {
                imageLoader.SetActive(false);

                this.profileImage.sprite = botSprite;

                return;
            }

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

}
