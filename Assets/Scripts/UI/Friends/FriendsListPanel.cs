using System.Collections;
using System.Collections.Generic;
using TrashTalk;
using UnityEngine;
using UnityEngine.UI;

public class FriendsListPanel : MonoBehaviour
{
    public Transform container;
    public GameObject friendItem;
    public Button inviteButton;
    public GameObject emptyDataText;

    [SerializeField] public List<User> selectedUsers;

    private void Start()
    {
        //    selectedList = new List<GameObject>();
        selectedUsers = new List<User>();
        //inviteButton.interactable = false;
        ShowList();
    }

    IEnumerator ShowList()
    {
        yield return null;
        WaitingLoader.instance.ShowHide(true);
        yield return null;
        ClearContianer(container);
        yield return null;

        List<FriendDetail> friends = PlayerProfile.instance.facebookFriends;

        //print("FRIENDS COUNT: " + friends.Count);
        print("FRIENDS COUNT: " + PlayerProfile.instance.facebookFriends);

        emptyDataText.SetActive(true);

        foreach (FriendDetail friend in friends)
        {
            yield return null;

            GameObject obj = Instantiate(friendItem, container, false);
            User user = new User();
            user.UserId = friend.friendUserID;
            user.FullName = friend.friendName;

            obj.GetComponent<FriendItem>().SetData(transform.GetSiblingIndex(), user, OnSelect);
        }

        yield return null;

        WaitingLoader.instance.ShowHide(false);

    }

    void OnSelect(User user, bool isSelected)
    {
        if (isSelected)
            selectedUsers.Add(user);
        else
            selectedUsers.Remove(user);

        //inviteButton.interactable = (selectedUsers.Count > 0);
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
