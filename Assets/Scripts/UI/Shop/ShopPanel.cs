using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using TrashTalk;

public class ShopPanel : UIPanel
{
   public override void Show()
   {
        gameObject.SetActive(true);
   }

   public override void Hide()
   {
        gameObject.SetActive(false);
   }

    public override void UpdateData(System.Action<object[]> callBack, params object[] parameters)
    {
        print(parameters[0]);
    }

    public void OnBuyCoins(int totalCoins, int price, string productID)
    {
        print("Purchasing " + totalCoins + " Coin");
        print("Cost: " + price);
        print("product: " + productID);

        PurchaseThroughInApp(totalCoins, price, productID);
    }

    int purchasedCoins = 0;
    void PurchaseThroughInApp(int totalCoins, int price, string productID)
    {
  //      InappManager.instance.PurchaseItem(productID, (payload, signature) => {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("UserID", PlayerProfile.Player_UserID);
            keyValuePairs.Add("ProductID", productID);
            keyValuePairs.Add("Price", price);
            keyValuePairs.Add("PurchaseCoins", totalCoins);

            this.purchasedCoins = totalCoins;

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.purchaseCoinsFunction, Method.POST, null, keyValuePairs, OnPurchaseSuccess, OnFail, CACHEABLE.NULL, true, null);

     //   });
    }

    void OnPurchaseSuccess(JObject resp, long arg2)
    {
        PlayerProfile.Player_coins += this.purchasedCoins;

        if (EventManager.UpdateUI != null)
            EventManager.UpdateUI.Invoke("UpdateCoins");
    }

    void OnFail(string msg)
    {
        print(msg);
    }

}
