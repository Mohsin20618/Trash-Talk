using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class PhotonRoomCreator : MonoBehaviourPunCallbacks
{
    private Hashtable _myCustomProperties = new Hashtable();


    public static bool IsPublicRoom = false;
    [Space]
    public string RoomID = "";

    [Header("Error Panel")]
    public GameObject roomFullPanel;

    [Header("Voice Player")]
    public VoicePlayer voicePlayer;
    public List<VoicePlayer> voicePlayers = new();

    //[Space]
    //[Header("PopUpAnimationController")]
    //public PopUpAnimationController popUpAnimationController;
    //[Space]
    //[Header("Lobby Screens")]
    //public GameObject loadingScreen;
    //public GameObject VsScreen;
    //[Space]
    //[Header("Player Details")]
    //public PlayerProfile playerProfile;
    //[Space]
    //[Header("ApiController")]
    //public ApiController apiController;
    //public static GameObject single;

    public static PhotonRoomCreator instance;

    private void Awake()
    {
        instance = this;
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby()");
        UpdateVoicePlayers();

        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom() Player: " + otherPlayer);
        Debug.Log("Player Left Room, Id is:  " + otherPlayer.UserId);
        GameplayManager.instance.ReplaceBotWithPlayer(otherPlayer.UserId);
        //  UpdatePlayerList();
        Debug.Log("IsMaster Left: " + otherPlayer.IsMasterClient);

    }

    /// <summary>
    /// Called when local user leave the room
    /// </summary>
    public override void OnLeftRoom()
    {
        if (!GameplayPanel.forceQuit)
        {
            Debug.LogError("Left room");
            MesgBar.instance.show("Connection Lost");
            if (Global.isMultiplayer && PhotonNetwork.InRoom)
            {
                LeavePhotonRoom();
            }
            SoundManager.Instance.StopBackgroundMusic();
            UIEvents.HidePanel(Panel.GameplayPanel);
            UIEvents.HidePanel(Panel.EndGamePanel);
            UIEvents.ShowPanel(Panel.TabPanels);
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("OnMasterClientSwitched: " + newMasterClient.UserId);

    }

    /// <summary>
    /// Main Method for creating Rooms
    /// </summary>
    /// <param name="isPublicRoom"> Room Category </param>
    private void CreateNewRoom(bool isPublicRoom = true, string roomName = "")
    {
        //if (PhotonNetwork.InLobby)
        {
            if (isPublicRoom == false)
            {
                if (roomName == "" || string.IsNullOrEmpty(roomName))
                {
                    Debug.LogError("Please Provide Room Name");
                    return;
                }
                foreach (var item in RoomListCaching.cachedRoomList)
                {
                    if (item.Value.Name.Equals(roomName))
                    {
                        if (item.Value.PlayerCount == item.Value.MaxPlayers)
                        {
                            Debug.LogError("Game already exist, but the room is full");
                            roomFullPanel.SetActive(true);
                            return;
                            //Open screen (Room is full)
                        }
                        else
                        {
                            Debug.LogError("Room already Exist, Joining to the room");
                            break;
                        }
                    }
                }
                WaitingLoader.instance.ShowHide(true);


                string gameVersion = "0.0.1";
                PhotonNetwork.GameVersion = gameVersion;//MasterManager.GameSettings.GameVersion;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 4;//byte.Parse(roomLimit_InputField.text);
                roomOptions.PublishUserId = true;
                roomOptions.IsVisible = isPublicRoom;
                roomOptions.IsOpen = true;
                _myCustomProperties["Host"] = PhotonNetwork.LocalPlayer.UserId;
                roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Host" };
                roomOptions.CustomRoomProperties = _myCustomProperties;
                RoomID = roomName.ToUpper();

                Debug.Log("RoomID: " + RoomID);

                PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
            }
            else
            {
                WaitingLoader.instance.ShowHide(true);

                if (string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName))
                {
                    PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
                }

                Debug.Log("Global.coinsRequired: " + Global.coinsRequired);
                string gameVersion = "0.0.1";
                PhotonNetwork.GameVersion = gameVersion;//MasterManager.GameSettings.GameVersion;
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 4;//byte.Parse(roomLimit_InputField.text);
                roomOptions.PublishUserId = true;
                roomOptions.IsVisible = isPublicRoom;
                roomOptions.IsOpen = true;
                //_myCustomProperties["Host"] = PhotonNetwork.LocalPlayer.UserId;
                _myCustomProperties["Coins"] = Global.coinsRequired.ToString();
                roomOptions.CustomRoomPropertiesForLobby = new string[1] { "Coins" };
                roomOptions.CustomRoomProperties = _myCustomProperties;
                //roomName = string.IsNullOrEmpty(roomName) ?"SAND_"+ randomNo:roomName;
                //RoomID = roomName.ToUpper();

                PhotonNetwork.JoinRandomOrCreateRoom(_myCustomProperties, 0, MatchmakingMode.FillRoom, TypedLobby.Default, null, null, roomOptions, null);
            }
        }
        //else
        //{
        //    Debug.LogError("Player Not in Lobby");
        //}
    }

    public void CreateRoomOnPhoton(bool publicRoom = true, string roomName = "")
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            CreateNewRoom(publicRoom, roomName);
        }
        else
        {
            if (!PhotonNetwork.IsConnected)
            {
                ReConnectingToPhoton();
            }
            else if (!PhotonNetwork.InLobby)
            {
                Debug.LogError("Player Not in Lobby");
            }
            else
            {
                Debug.LogError("Some thing else happend");
            }
        }
    }

    public void ReConnectingToPhoton()
    {
        Debug.Log("ConnectingToPhoton. . .");

        string gameVersion = "0.0.1";
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.UserId = PlayerProfile.Player_UserID; // alternatively set by server
        PhotonNetwork.LocalPlayer.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.NickName = PlayerProfile.Player_UserName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom successfully: Room ID: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("OnJoinRoomFailed: " + message);
        if (!PhotonNetwork.InLobby)
        {
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("GameObject Call LeaveRoom: " + gameObject.name, gameObject);
            PhotonNetwork.LeaveRoom();
            TypedLobby Default = new TypedLobby("US", LobbyType.Default);
            PhotonNetwork.JoinLobby(Default);
        }

        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }
    }

    public void LeavePhotonRoom()
    {
        PhotonNetwork.LeaveRoom();
        PlayerManager.instance.ClearPlayers();
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom()");
        PlayerProfile.RoomId = RoomID;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            RoomOptions roomOptions = new RoomOptions();
            _myCustomProperties["RoomId"] = RoomID;
            roomOptions.CustomRoomPropertiesForLobby = new string[1] { "RoomId" };
            roomOptions.CustomRoomProperties = _myCustomProperties;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_myCustomProperties);
        }
        PlayerManager.instance.ClearPlayers();
        UpdateVoicePlayers();
        //UpdatePlayerList();
        MasterList();



        if (WaitingLoader.instance.gameObject.activeInHierarchy)
        {
            WaitingLoader.instance.ShowHide(false);
        }

        Debug.Log("Joined a room");
        Global.isMultiplayer = true;
        UIEvents.HidePanel(Panel.GameplayPanel);
        UIEvents.HidePanel(Panel.TabPanels);
        UIEvents.HidePanel(Panel.FriendsPanel);
        UIEvents.ShowPanel(Panel.GameplayPanel);

        //Hunain
        VoiceManager.instance.EnableDisableVoiceManager();
        ChatHandler.instance.EnableDisableChatManager(true);
    }

    #region new code
    public void setPhotonProps()
    {
        string name = PlayerProfile.Player_UserName;
        // string pic = TextureConverter.Texture2DToBase64(PlayerProfile.Player_rawImage_Texture2D);
        string imageUrl = PlayerProfile.imageUrl;
        string email = PlayerProfile.Player_Email;
        string country = CountryCode.code;
        Hashtable hash = new Hashtable();
        hash["Name"] = name;
        //   hash["Picture"] = pic;
        hash["Url"] = imageUrl;
        hash["Email"] = email;
        hash["WonCount"] = PlayerProfile.gamesWon.ToString();
        hash["Level"] = PlayerProfile.level.ToString();
        hash["GamesPlayed"] = PlayerProfile.gamesPlayed.ToString();
        hash["Country"] = country;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    #endregion

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed()");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed: " + message);
    }


    private void MoveToWaitingScreen()
    {
        Debug.Log("MoveToWaitingScreen()");
        //GetOponentPicture();
        //popUpAnimationController.PingPongAnimation(loadingScreen);
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        //{
        //    StartCoroutine(Show_Vs_Screen_To_LocalPlayer());
        //}
    }


    public static List<string> masterList = new();

    public void MasterList()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //Generate List and Show
                GameplayManager.instance.partnerPanel.SetData(PhotonNetwork.PlayerList.ToList());
            }
        }
    }

    public void MasterListReceive(string[] playerIds)
    {
        masterList = playerIds.ToList();
        UpdatePlayerList();
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.LogError("OnPlayerEnteredRoom playerName: " + newPlayer.NickName);

        PlayerManager.instance.ClearPlayers();
        UpdateVoicePlayers();
        //UpdatePlayerList();
        MasterList();
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            GameplayManager.instance.ShowMultiplayerMessage(false);
            GameplayManager.instance.SetPlayButton(true);
        }

        //if (newPlayer != PhotonNetwork.LocalPlayer)
        //{
        //    StartCoroutine(MoveToVs_Screen());
        //}
    }

    void UpdatePlayerList()
    {
        //  Global.playerData = new System.Collections.Generic.List<MutiplayerData>();
        //   PlayerManager.instance.AddPlayer("Player 1", false, true, 0);

        for (int i = 0; i < 4; i++)
        {
            PlayerManager.instance.AddPlayer($"Waiting..", null, null, false, false, true, 0);
            PlayerManager.instance.players[i].photonIndex = i;
        }


        for (int i = 0; i < masterList.Count; i++)
        {
            if (i >= masterList.Count)
            {
                break;
            }
            if (string.IsNullOrEmpty(masterList[i]))
            {
                continue; 
            }

            if (PlayerManager.instance.players[i].id == null)
            {
                object imageUrl;
                string url = "";
                object objWonCount;
                string WonCount = "";
                object objLevel;
                string Level = "";
                object objGamesPlayed;
                string GamesPlayed = "";
                object objCountry;
                string Country = "";

                Photon.Realtime.Player player = PhotonNetwork.PlayerList.ToList().Find(x => x.UserId.Equals(masterList[i]));
                if (player.CustomProperties.TryGetValue("Url", out imageUrl))
                {
                    url = (string)imageUrl;
                }

                if (player.CustomProperties.TryGetValue("WonCount", out objWonCount))
                {
                    WonCount = (string)objWonCount;
                }


                if (player.CustomProperties.TryGetValue("Level", out objLevel))
                {
                    Level = (string)objLevel;
                }


                if (player.CustomProperties.TryGetValue("GamesPlayed", out objGamesPlayed))
                {
                    GamesPlayed = (string)objGamesPlayed;
                }


                if (player.CustomProperties.TryGetValue("Country", out objCountry))
                {
                    Country = (string)objCountry;
                }


                PlayerManager.instance.players[i].name = player.NickName;
                PlayerManager.instance.players[i].id = player.UserId;

                PlayerManager.instance.players[i].imageURL = url;

                PlayerManager.instance.players[i].wonCount = WonCount;
                PlayerManager.instance.players[i].level = Level;
                PlayerManager.instance.players[i].gamesPlayed = GamesPlayed;
                PlayerManager.instance.players[i].country = Country;

                PlayerManager.instance.players[i].isOwn = player.UserId.Equals(PhotonNetwork.LocalPlayer.UserId);
                PlayerManager.instance.players[i].isMaster = player.IsMasterClient;
                PlayerManager.instance.players[i].isBot = false;
                //PlayerManager.instance.players[i].tablePosition = 0;
                PlayerManager.instance.players[i].photonIndex = i;


            }

            //   PlayerManager.instance.AddPlayer($"Waiting..", null, null, false, false, true, 0);
        }

        //foreach (var item in PhotonNetwork.PlayerList)
        //{
        //    object imageUrl;
        //    string url = "";
        //    if (item.CustomProperties.TryGetValue("Url", out imageUrl))
        //    {
        //        url = (string)imageUrl;
        //    }
        //    PlayerManager.instance.AddPlayer(item.NickName, item.UserId, url, item.UserId.Equals(PhotonNetwork.LocalPlayer.UserId), item.IsMasterClient, false, 0);
        //}

        print("Before going next: the count is: " + PlayerManager.instance.players.Count);




        PlayerManager.instance.players = PlayerManager.instance.SortMultiplayerPositions();

        //for (int j = 0; j < PlayerManager.instance.players.Count; j++)
        //{
        //    print("After RRR: " + j + " :  " + PlayerManager.instance.players[j].id);
        //}



        UIEvents.UpdateData(Panel.PlayersUIPanel, null, "SetPlayersData");

        //   Invoke("ttt", 0.5f);
    }

    private void UpdateVoicePlayers()
    {
        Debug.Log("UpdateVoicePlayers");
        foreach (var item in voicePlayers)
        {
            Destroy(item.gameObject);
        }
        voicePlayers.Clear();

        if (!PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
            return;
        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            if (item.Value.IsLocal)
            {
                continue;
            }
            var player = Instantiate(voicePlayer.gameObject);
            var obj = player.GetComponent<VoicePlayer>();
            voicePlayers.Add(obj);
            obj.SetData(item.Value);
        }
    }

    public IEnumerator MoveToVs_Screen()
    {
        Debug.LogError("MoveToVs_Screen()");
        yield return null;
        ////yield return new WaitForSeconds(3f);
        //popUpAnimationController.PingPongAnimation(loadingScreen);
        //popUpAnimationController.PingPongAnimation(VsScreen);
        ////yield return new WaitForSeconds(4f);
        //yield return null;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.LoadLevel(1);
        //}

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("GameId"))
        {
            Global.currentGameId = propertiesThatChanged["GameId"].ToString();
            print("Global.currentGameId is " + Global.currentGameId);
        }
        //  print("propertiesThatChanged: " + propertiesThatChanged);
        // propertiesThatChanged["GameId"].ToString();
    }
}
