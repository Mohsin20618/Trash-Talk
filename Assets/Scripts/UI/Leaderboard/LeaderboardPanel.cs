using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TrashTalk;


public class LeaderboardPanel : UIPanel
{
    public Transform container;
    public GameObject itemPrefab;

    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {

    }

    private void OnEnable()
    {
        WebServiceManager.instance.APIRequest(WebServiceManager.instance.leaderboard, Method.GET, null, null, OnSuccess, OnFail, CACHEABLE.NULL, true, null);
    }

    void OnSuccess(JObject resp, long arg2)
    {
        var users = LeaderboardUsers.FromJson(resp.ToString());

        List<User> usersList = users.data;

        Debug.Log("Leaderboard users: " + usersList.Count);

        ShowData(usersList);
    }

    void ShowData(List<User> users)
    {
        ClearContianer(container);
        int index = 0;
        foreach (User user in users)
        {
            index++;
            GameObject obj = Instantiate(itemPrefab, container, false);
            obj.GetComponent<LeaderboardItem>().SetData(index, user);
        }
    }

    void OnFail(string msg)
    {
        print("Leaderboard Fail: " + msg);
    }

    public void OnBackButton()
    {
        UIEvents.ShowPanel(Panel.FriendsPanel);
        Hide();

    }

    void ClearContianer(Transform container)
    {
        foreach (Transform child in container)
            Destroy(child.gameObject);
    }
}
