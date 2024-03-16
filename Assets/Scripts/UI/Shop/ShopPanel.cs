using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using TrashTalk;
using UnityEngine.UI;
using System.Collections;

public class ShopPanel : UIPanel
{
    public Button leftBtn; public Button rightBtn;
    public ScrollRect scrollView;

    public float scrollAmount = 0.1f; // Adjust this value for the amount to scroll
    public float scrollSpeed = 0.5f; // Adjust this value for desired smoothness
    private bool isScrolling = false;

    private void Start()
    {
        // Add listeners to the buttons
        rightBtn.onClick.AddListener(ScrollRight);
        leftBtn.onClick.AddListener(ScrollLeft);
    }

    private void ScrollRight()
    {
        if (!isScrolling)
            StartCoroutine(ScrollSmoothly(Vector2.right));
    }

    private void ScrollLeft()
    {
        if (!isScrolling)
            StartCoroutine(ScrollSmoothly(Vector2.left));
    }

    private IEnumerator ScrollSmoothly(Vector2 direction)
    {
        isScrolling = true;
        float elapsedTime = 0f;
        Vector2 startPosition = scrollView.normalizedPosition;
        Vector2 endPosition = startPosition + direction * scrollAmount;

        while (elapsedTime < scrollSpeed)
        {
            scrollView.normalizedPosition = Vector2.Lerp(startPosition, endPosition, (elapsedTime / scrollSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollView.normalizedPosition = endPosition;
        isScrolling = false;
    }

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
        UIEvents.ShowPanel(Panel.GameSelectPanel);
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
        //InappManager.instance.PurchaseItem(productID, (payload, signature) =>
        //{
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            keyValuePairs.Add("UserID", PlayerProfile.Player_UserID);
            keyValuePairs.Add("ProductID", productID);
            keyValuePairs.Add("Price", price);
            keyValuePairs.Add("PurchaseCoins", totalCoins);

            this.purchasedCoins = totalCoins;

            WebServiceManager.instance.APIRequest(WebServiceManager.instance.purchaseCoinsFunction, Method.POST, null, keyValuePairs, OnPurchaseSuccess, OnFail, CACHEABLE.NULL, true, null);

        //});
    }

    void OnPurchaseSuccess(JObject resp, long arg2)
    {
        PlayerProfile.Player_coins += this.purchasedCoins;

        if (EventManager.UpdateUI != null)
            EventManager.UpdateUI.Invoke("UpdateCoins");

        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Purchase Successfully", "", "OK");

    }

    void OnFail(string msg)
    {
        print(msg);
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, null, "SetData", "Purchase Fail: " + msg, "", "OK");
    }

}
