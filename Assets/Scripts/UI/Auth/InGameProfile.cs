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
        ImageCacheManager.instance.CheckOrDownloadImage(player.imageURL, countryPicture);
    }

    public void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }
}
