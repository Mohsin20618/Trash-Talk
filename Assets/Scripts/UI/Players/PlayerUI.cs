using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using System.Linq;

public class PlayerUI : MonoBehaviour
{
    public PlayerBidUI bidUI;
    public GameObject cardCountContainer;
    public Text myBids;
    public Text cardsCountText;
    public Text bidCount;

    public TextMeshProUGUI nameText;
    public Text gameScore;
    public Image profileImage;
    public GameObject imageLoader;
    public Image profileBG;

    [Space]
    [Header("Chat Spawn Transform")]
    public Transform spawnPoint;
    [Space]
    [Header("For Audio Input")]
    public string userId = "";
    public Image muteIcon;
    public Button muteBtn;

    public Image bonusImage;
    public GameObject timerObj;
    
    //private string profileImageURL = "https://i.pravatar.cc/300";

    System.Action<int> callBack;

    //public MutiplayerData playerData;

    private void OnEnable()
    {
        if(Global.isMultiplayer)
            SetUI();
        else
            SetUI("Bot");
    }

    private void Start()
    {
       if(Global.isMultiplayer)
            muteBtn.onClick.AddListener(()=> MicBtnListener());
    }


    public void MicBtnListener()
    {
        bool enableOrDisable = false;

        if (userId.Equals(PhotonNetwork.LocalPlayer.UserId)) //Disable Mic Transmission of myself
        {

            enableOrDisable = VoiceManager.instance.EnableDisableAudioTransmition();
        }
        else //Disable Other Player Audio Source
        {

            VoicePlayer voicePlayer = PhotonRoomCreator.instance.voicePlayers.Find(x => x.userId.Equals(this.userId));

            
            if(voicePlayer == null)
                voicePlayer = FindObjectsOfType<VoicePlayer>().ToList().Find(x => x.userId.Equals(this.userId));

            if (voicePlayer)
            {
                Debug.Log("Disable voice for: " + voicePlayer.userId);
                voicePlayer.EnableDisableAudioSource();
            }
            else
            {
                Debug.Log("Voice Player not found: " + userId);
            }
            enableOrDisable = voicePlayer.audioSource.enabled;
        }
        
        
        //Color color = muteIcon.color;
        //color.a = enableOrDisable ? 1 : 0.5f;
        //muteIcon.color = color;
    }

    public void SetUI(string name="Waiting...",string userId="",Sprite botSprite=null, int score=0, string imageUrl=null)
    {
        //Hunain
        this.userId = userId;
        if (!string.IsNullOrEmpty(userId) && Global.isMultiplayer)
        {
            if (userId.Equals(PhotonNetwork.LocalPlayer.UserId))
            {
                VoiceManager.instance.ResetMicIcon();
                muteIcon.gameObject.SetActive(true);
            }
        }
        else
        {
            muteIcon.gameObject.SetActive(false);
        }
        //Hunain End

        if (nameText!=null)
          nameText.text = name;
        if (gameScore != null)
            gameScore.text = score.ToString();


        if(profileImage !=null)
        {
            //Profile work
            if (botSprite !=null)
            {
                imageLoader.SetActive(false);

                this.profileImage.sprite = botSprite;

                return;
            }

            if (imageUrl != null && imageUrl != "")
            {
                ImageCacheManager.instance.CheckOrDownloadImage(imageUrl, this.profileImage, DownloadCallBack);
            }
            else
                imageLoader.SetActive(false);
        }


    }


    public void DisplayChatMsg(string chatTypeStr, int index)
    {
        ChatType chatType;
        if (Enum.TryParse<ChatType>(chatTypeStr, out chatType))
        {
            GameObject prefab;
            if (chatType.Equals(ChatType.emoji))
            {
                Sprite sprite = ChatHandler.instance.emojis[index];
                prefab = Instantiate(ChatHandler.instance.emojiPrefab, spawnPoint, false);
                prefab.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            }
            else
            {
                string msg = ChatHandler.instance.texts[index].text;
                prefab = Instantiate(ChatHandler.instance.textPrefab, spawnPoint, false);
                prefab.transform.GetComponentInChildren<Text>(true).text = msg;
                Debug.Log("msg: " + msg);
            }

            prefab.transform.position = Vector3.zero;
            prefab.transform.rotation = Quaternion.identity;
            prefab.transform.localScale = Vector3.one;

            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = Quaternion.identity;
            prefab.transform.localScale = Vector3.one;
            StartCoroutine(_DestroyChat(prefab));

        }
    }

    IEnumerator _DestroyChat(GameObject prefab)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(prefab);
    }

    public void ShowBonusImage(Sprite sprite)
    {
        bonusImage.gameObject.SetActive(true);

        if(sprite != null)
            bonusImage.sprite = sprite;
    }

    void DownloadCallBack(Texture2D texture2D)
    {
        imageLoader.SetActive(false);
    }

    public void UpdateCardCount(int cardCount)
    {
//        print(name);
        cardCountContainer.SetActive(cardCount > 0);

        cardsCountText.text = $"{cardCount}";
    }

   public void ShowBidUI(int bidCount = -1,System.Action<int> callBack=null)
   {
        this.callBack = callBack;
        this.bidUI.gameObject.SetActive(true);
        this.bidUI.UpdateUI(bidCount, SelectBid);
   }

    void SelectBid(int bid)
    {
      //  myBids.text = $"{bid}";
        this.bidUI.gameObject.SetActive(false);
        this.callBack(bid);
    }

    public void UpdateBids(int totalBids, int bidWon)
    {

        if (bidCount != null)
            bidCount.text = $"{bidWon}/{totalBids}";

        if (myBids != null)
            myBids.text = $"{bidWon}/{totalBids}";
    }

    public void ResetUI()
    {
        Debug.Log("ResetUI");
        timerObj.SetActive(false);
        if (bidCount != null)
            bidCount.text = "";
        if (myBids != null)
            myBids.text = "0";

        if(cardCountContainer)
            cardCountContainer.SetActive(false);

        bonusImage.gameObject.SetActive(false);
        HideBidUI();
    }

    public void WinAnimation()
    {
        LeanTween.scale(gameObject, new Vector2(1.1f,1.1f), 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(()=> {
            LeanTween.scale(gameObject, Vector2.one, 0.5f).setEase(LeanTweenType.easeInOutQuad);

        });
    }

    //Waiting for master to start the game
    //wating for other to join the game
    //Are you sure you want to logout
    public void SetTurnIndication(bool isTurn)
    {
        //CancelInvoke(nameof(PlayTimerSound));
        //SoundManager.Instance.StopTimerEffect();
        timerObj.SetActive(isTurn);
        //if(isTurn) Invoke(nameof(PlayTimerSound),23f);
        if (profileBG == null)
            return;

        profileBG.color = isTurn?Color.red:Color.white;
    }

    public void StopTimer()
    {
        //CancelInvoke(nameof(PlayTimerSound));
        //SoundManager.Instance.StopTimerEffect();
        //CancelInvoke(nameof(PlayTimerSound));
        timerObj.SetActive(false);
    }


    //void PlayTimerSound()
    //{
    //    Debug.Log("Timer Sound On");
    //    SoundManager.Instance.PlayTimerEffect();
    //}

    public void HideBidUI()
    {
        this.bidUI.gameObject.SetActive(false);
    }
}
