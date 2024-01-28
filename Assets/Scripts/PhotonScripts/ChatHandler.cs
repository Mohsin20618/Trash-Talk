using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatHandler : MonoBehaviour
{

    public Button chatBtn;
    public Button emojiBtn;

    public GameObject chatPanel;
    public GameObject emojiPanel;

    public static ChatHandler instance;

    public List<Sprite> emojis = new();
    public List<Text> texts = new();


    public GameObject emojiPrefab;
    public GameObject textPrefab;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        chatBtn.onClick.AddListener(()=> OpenChatPanel());
        emojiBtn.onClick.AddListener(()=> OpenEmojiPanel());
    }

    private void OpenChatPanel()
    {
        CloseChatPanel();
        CloseEmojiPanel();

        chatPanel.SetActive(true);
    }

    private void OpenEmojiPanel()
    {
        CloseChatPanel();
        CloseEmojiPanel();

        emojiPanel.SetActive(true);
    }

    private void CloseChatPanel()
    {
        chatPanel.SetActive(false);
    }

    private void CloseEmojiPanel()
    {
        emojiPanel.SetActive(false);
    }

    public void SendChatMessage(ChatType chatType, int index)
    {
        // Send an RPC to all players to display the message
        CloseChatPanel();
        CloseEmojiPanel();
        PhotonRPCManager.Instance.SendRPC("RPC_DisplayChatMessage" , RpcTarget.Others, PhotonNetwork.LocalPlayer.UserId, chatType.ToString(), index);

    }

    public void DisplayMessage(string senderId, string chatTypeStr, int index)
    {
        Debug.Log("DisPlay Message");
        PlayerUI playerUI = FindObjectsOfType<PlayerUI>().ToList().Find(x => x.userId.Equals(senderId));
        if (playerUI)
            playerUI.DisplayChatMsg(chatTypeStr, index);
    }

    internal void EnableDisableChatManager(bool state)
    {
        gameObject.SetActive(state);
        chatBtn.gameObject.SetActive(state);
        emojiBtn.gameObject.SetActive(state);
    }
}

public enum ChatType
{
    text,
    emoji
}