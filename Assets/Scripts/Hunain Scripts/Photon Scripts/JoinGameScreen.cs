using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameScreen : MonoBehaviour
{
    public InputField joinGameIF;
    public Button joinGameBtn;


    // Start is called before the first frame update
    void Start()
    {
        joinGameBtn.onClick.AddListener(()=> JoinGameCallback());
    }

    private void JoinGameCallback()
    {
        PhotonRoomCreator.instance.JoinPrivateRoom(joinGameIF.text.ToUpper().Trim());
    }
    public void UpdateBtnInteractability()
    {
        joinGameBtn.interactable = (joinGameIF.text.Trim().Length > 0);
    }
}
