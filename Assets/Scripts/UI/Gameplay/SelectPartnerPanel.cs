using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPartnerPanel : MonoBehaviour
{
    public Transform container;
    System.Action<string> onSelect;

    private void OnEnable()
    {
        DisableAll();
       // SetData(2);
    }

    public void SetData(List<Player> players, System.Action<string> onSelect)
    {
        this.onSelect = onSelect;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].isBot || players[i].id == PlayerProfile.Player_UserID)
                continue;

            if (i <= container.childCount)
            {
                container.GetChild(i).gameObject.SetActive(true);
                container.GetChild(i).GetComponent<PartnerObject>().SetData(OnSelect, players[i].name, players[i].id, players[i].imageURL);
            }
        }
    }

    void DisableAll()
    {
        foreach(Transform child in container)
        {
            child.gameObject.SetActive(false);
        }
    }

    void OnSelect(string id)
    {
        this.onSelect(id);
        this.gameObject.SetActive(false);
    }

    public void OnSkipButton()
    {
        OnSelect("");
    }

}
