using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionCard : MonoBehaviour
{   public int entryCoins;
    public Text entryFees;

    private void OnEnable()
    {
        if (entryCoins == -1)
        {
            entryFees.text = PlayerProfile.Player_coins.ToString(); //All In
        }
    }
    public void OnSelectRoom()
    {
        if (entryCoins == -1) //All in
        {
            if (PlayerProfile.Player_coins < 1)
            {
                MesgBar.instance.show("You don't have enough coins.", true);
                return;
            }
            entryCoins = (int)PlayerProfile.Player_coins; //All in
        }
        else
        {
            if (PlayerProfile.Player_coins < entryCoins)
            {
                MesgBar.instance.show("You don't have enough coins.", true);
                return;
            }
        }


        Global.coinsRequired = entryCoins;

        if (Global.isPublicRoom)
        {
           PhotonRoomCreator.instance.CreateRoomOnPhoton(Global.isPublicRoom, "");
        }

        print("ROOM SELECTED WITH COINS: " + Global.coinsRequired);

        UIEvents.HidePanel(Panel.TabPanels);

        if (Global.isPublicRoom)
        {
            UIEvents.ShowPanel(Panel.GameplayPanel);
        }
        else
        {
            UIEvents.ShowPanel(Panel.FriendsPanel);
        }

        //Hunain
        VoiceManager.instance.EnableDisableVoiceManager();
        ChatHandler.instance.EnableDisableChatManager(false);
    }
}
