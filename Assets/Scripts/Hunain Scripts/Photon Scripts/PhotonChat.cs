using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;


public class PhotonChat : MonoBehaviourPunCallbacks, IChatClientListener
{

    public ChatAppSettings chatAppSettings;
    internal string friendID;
    public string roomID;
    public string GameID;
    public string SenderID;

    public PlayOrReject playOrReject_Ref;
    public static PhotonChat Instance;

    //public Controller controller;
    public string Name { get; private set; }

    public static GameObject single;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (single && single != this.gameObject)
        {
            /// If a singleton already exists, destroy the old one - TODO: Not sure about this behaviour yet. Allows for settings changes with scene changes.
            Destroy(single);
        }

        single = this.gameObject;
        DontDestroyOnLoad(this.gameObject);
    }

    internal void GameObjectsSetter()
    {
        //if(controller == null)
        //{
        //    controller = FindObjectOfType<Controller>();
        //}
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state);
        if (state == ChatState.ConnectedToNameServer)
        {
            Debug.Log("Chat Connected");
            PhotonConnectionController.isChatConnected = true;
        }
        if (state == ChatState.Disconnected)
        {
            PhotonConnectionController.isChatConnected = false;
        }
        //throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        PhotonConnectionController.isChatConnected = false;
        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log(messages);
        //throw new System.NotImplementedException();
    }
     
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log(message);
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log(results);
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {

        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }
    public ChatClient chatClient;
   
    [ContextMenu("Connect")]
    internal void Connect()
    {

        chatClient = new ChatClient(this);
        chatClient.AuthValues = new AuthenticationValues();
        chatAppSettings.AppIdChat = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat;
        chatAppSettings.FixedRegion = PhotonNetwork.CloudRegion;
        chatAppSettings.AppVersion = PhotonNetwork.AppVersion;
        chatAppSettings.Server = PhotonNetwork.Server.ToString();
        chatClient.Service();
        chatClient.Connect(chatAppSettings.AppIdChat, chatAppSettings.AppVersion, new AuthenticationValues(PhotonNetwork.LocalPlayer.UserId));

    }

    internal void RequestRejected(string targetUserID, string message)
    {
        chatClient.SendPrivateMessage(targetUserID, message);
    }

    System.Action PUNCallBack;
    internal void RequestAndSendMessage(string targetUserID, string roomID, System.Action callBack=null)
    {
        PUNCallBack = callBack;
        StartCoroutine(RequestAndSendMessage_Co(targetUserID, roomID));
    }

    internal IEnumerator SendSeatNumber(string targetUserID, string seatNum)
    {
        if (PhotonNetwork.InRoom)
        {
            yield return null;
            chatClient.SendPrivateMessage(targetUserID, seatNum);
        }
    }

    internal IEnumerator RequestAndSendMessage_Co(string targetUserID, string roomID)
    {
        string messagetoSend = "requested," + targetUserID + "," + roomID + "," + PlayerProfile.GameId;
        print("Gameid is" + PlayerProfile.GameId);
        yield return new WaitUntil(()=> PhotonNetwork.InRoom);
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("RequestAndSendMessage_Co: targetUserID: "  + targetUserID);
            chatClient.SendPrivateMessage(targetUserID, messagetoSend);
            yield return null;
        }
    }

    internal void AcceptGameInvitation(string senderId, string roomId)
    {
        //  if (PhotonNetwork.IsConnected)
        //  {
        //Debug.Log("Requested Accepted by: "  + senderId);
        //string messagetoSend = "accepted," + roomId;
        //chatClient.SendPrivateMessage(senderId, messagetoSend);
        //  }
        WaitingLoader.instance.ShowHide(true);
        PhotonNetwork.JoinRoom(roomId);
    }

    internal IEnumerator SendJoinRequest(string targetUserID, string action,GameObject prefab =null)
    {
        print("did came here?");
            if (chatClient == null) Connect();
            yield return new WaitUntil(() => chatClient != null);
            Debug.Log("SendJoinRequest PhotonNetwork.InRoom");
            yield return null;
            switch (action)
            {
                case "requested":
                    chatClient.SendPrivateMessage(targetUserID, "requested");
                    break;
                case "accepted":
                    chatClient.SendPrivateMessage(targetUserID, "accepted" + "," + PhotonNetwork.CurrentRoom.Name);
                    break;
                case "rejected":
                    chatClient.SendPrivateMessage(targetUserID, "rejected");
                    break;
                default:
                    break;
            }
        if(prefab!=null) Destroy(prefab, 2f);
    }

    private void Update()
    {
        if (chatClient != null) { chatClient.Service(); }

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //set some loading screen true where there should be option to either play or reejct the request

        Debug.Log("OnPrivateMessage Recieved: sender id is " + sender.ToString() + " message.  " + message.ToString());
        
        string[] msg = message.ToString().Split(',');
        print("message name: " + msg[0]);
        if (msg[0] == "requested" && sender.ToString() == PlayerProfile.Player_UserID) // i was the sender
        {
            this.PUNCallBack();
            Debug.Log("Game Request Sent");
            return;
        }
        else if (msg[0] == "requested" && sender.ToString() != PlayerProfile.Player_UserID) // i was the reciever in below cases
        {
            Debug.Log("Game Request recieved");
           // if (message.ToString() == "requested")
           // {
                OnGetGameRequest(sender, msg[2]);
                Debug.Log("Request received from " + sender.ToString()); 

          //  }
            return;
        }
        else if (msg[0]=="accepted" && sender.ToString() == PlayerProfile.Player_UserID)
        {
            Debug.Log("Request has been accepted by " + sender.ToString());
            print("RoomID: " + msg[1]);
            PhotonNetwork.JoinRoom(msg[1]);

          //  var roomName = message.ToString().Split(',');
            return;
        }
        else if (msg[0] == "rejected" && sender.ToString() != PlayerProfile.Player_UserID)
        {
          //  if (message.ToString() == "rejected")
          //  {
                Debug.Log("Request has been rejected by " + sender.ToString());
          //  }
            return;
        }
    }

    public void getProfileData()
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        keyValuePairs.Add("userID", PlayerProfile.Player_UserID);

        WebServiceManager.instance.APIRequest(WebServiceManager.instance.getProfileFunction, Method.GET, null, keyValuePairs, null, null, CACHEABLE.NULL, true, null);
    }

    public void OnGetGameRequest(string senderId, string roomId)
    {
        print("Got Invitation from: " + senderId);
        print("Invitation for Room Id: " + roomId);


        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (data) => {

            if ((int)data[0] == 2)//on yes
            {

                if (PlayerProfile.Player_coins < Global.coinsRequired)
                {
                    Invoke("OnLessCoins", 0.05f);
                    return;
                }
                print("Accepted");

                PhotonChat.Instance.AcceptGameInvitation(PlayerProfile.Player_UserID, roomId);
            }
            else
            {
                print("Rejected");

            }

        }, "SetData", "You have received game invitation", "Reject", "Accept");
    }

    void OnLessCoins()
    {
        UIEvents.ShowPanel(Panel.Popup);
        UIEvents.UpdateData(Panel.Popup, (data) =>
        {


        }, "SetData", $"You need at least {Global.coinsRequired} Coins!", "", "OK");
    }
}
