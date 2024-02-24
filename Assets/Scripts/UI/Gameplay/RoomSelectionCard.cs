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
            entryCoins = (int)PlayerProfile.Player_coins;
        }
        Global.coinsRequired = entryCoins;


        PhotonRoomCreator.instance.CreateRoomOnPhoton(true, "");

        print("ROOM SELECTED WITH COINS: " + Global.coinsRequired);

        UIEvents.HidePanel(Panel.TabPanels);
        UIEvents.ShowPanel(Panel.GameplayPanel);

        //Hunain
        VoiceManager.instance.EnableDisableVoiceManager();
        ChatHandler.instance.EnableDisableChatManager(false);
    }
}
