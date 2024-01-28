using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int price;
    public int totalCoins;
    public string productID;

    public Text priceText;
    public Text coinsText;
    

    public ShopPanel shop;

    private void Start()
    {
        priceText.text = $"$ {price}";
        coinsText.text = $"{totalCoins}";
    }


    public void OnBuyButton()
    {
        shop.OnBuyCoins(totalCoins, price, productID);
    }

}
