using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionCard : MonoBehaviour
{   public int entryCoins;
    public void OnSelectRoom()
    {
        if (entryCoins == -1)
        {
            entryCoins = (int)PlayerProfile.Player_coins; //All in
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
