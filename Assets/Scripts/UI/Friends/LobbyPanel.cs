using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TrashTalk;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;

    List<GameObject> selectedList;

    private void Start()
    {
        selectedList = new List<GameObject>();
        //WebServiceManager.instance.APIRequest(WebServiceManager.instance.getLatestPlayedMembers, Method.GET, null, null, OnSuccess, OnFail, CACHEABLE.NULL, true, null);
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getFriends, Method.GET, null, null, OnSuccess, OnFail, CACHEABLE.NULL, true, null);

    }

    private void OnSuccess(JObject data, long arg2)
    {
        Debug.Log("RECENT FRIENDS" + data);

        ShowList();
    }
    private void OnFail(string obj)
    {
        Debug.Log("RECENT FRIENDS FAILED");
    }

    void ShowList()
    {
        ClearContianer(container);

        for (int i = 0; i < 7; i++)
        {
  //          GameObject obj = Instantiate(friendItem, container, false);
//            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(),OnSelect);
        }
    }

    void OnSelect(GameObject item,bool isSelected)
    {
        if (isSelected)
            selectedList.Add(item);
        else
            selectedList.Remove(item);
    }

    public void OnStartButton()
    {
        UIEvents.ShowPanel(Panel.GameplayPanel);
        UIEvents.HidePanel(Panel.FriendsPanel);
    }

    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
