using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameProfile : MonoBehaviour
{
    public Image playerPicture;
    public Text playerName;
    public Text playerCountry;
    public Image countryPicture;
    public Text playerTotalGames;
    public Text playerWins;
    public Text playerLevel;
    public Button closeButton;
    public string userId;

    public List<Countries> countries = new();

    private void Start()
    {
        closeButton.onClick.AddListener(ClosePopup);    
    }

    public void OpenProfile(Player player)
    {
        playerName.text = player.name;
        playerCountry.text = player.country;
        playerTotalGames.text = player.gamesPlayed;
        playerWins.text = player.wonCount;
        playerLevel.text = player.level;
        this.userId = player.id;
        ImageCacheManager.instance.CheckOrDownloadImage(player.imageURL, playerPicture);

        Sprite flagSprite = GetFlag();
        countryPicture.sprite = flagSprite != null ? flagSprite : countries[0].countryFlag;
    }

    Sprite GetFlag()
    {
        foreach (var item in countries)
        {
            if (item.countryCode.ToLower().Trim() == CountryCode.code.ToLower().Trim())
            {
                Debug.Log("ss ");
                return item.countryFlag;
            }
        }

        return null;
    }
    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
