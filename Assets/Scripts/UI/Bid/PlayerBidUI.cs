using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerBidUI : MonoBehaviour
{
    public Text bidCount;

    System.Action<int> callBack;

    public void UpdateUI(int bid,System.Action<int> callBack)
    {
        this.callBack = callBack;

        if (bidCount != null)
            bidCount.text = $"I Bid {bid}";
    }

    public void SelectBid(int bid)
    {
        this.callBack(bid);
    }
}
