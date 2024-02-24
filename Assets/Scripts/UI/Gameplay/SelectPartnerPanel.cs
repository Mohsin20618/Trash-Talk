using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPartnerPanel : MonoBehaviour
{
    public Transform container;
    public List<PartnerObject> partnerObjects;

    private void OnEnable()
    {
    }

    public void EnableDisablePanel(bool state) { gameObject.SetActive(state); }

    public void SetData(List<Photon.Realtime.Player> players)
    {
        EnableDisablePanel(true);
        object imageUrl;
        string url = "";

        foreach (var item in partnerObjects)
        {
            item.gameObject.SetActive(false);
        }


        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].IsMasterClient)
            {
                continue;
            }

            if (players[i].CustomProperties.TryGetValue("Url", out imageUrl))
            {
                url = (string)imageUrl;
            }
            partnerObjects[i].SetData(players[i].NickName, players[i].UserId, url);

            partnerObjects[i].gameObject.SetActive(true);
        }
    }
}
