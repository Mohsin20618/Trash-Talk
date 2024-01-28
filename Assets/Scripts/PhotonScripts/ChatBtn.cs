using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBtn : MonoBehaviour
{
    public int index;
    public ChatType chatType;
    Button thisBtn;

    private void Awake()
    {
        thisBtn = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        thisBtn.onClick.AddListener(()=> ChatHandler.instance.SendChatMessage(chatType, index));    
    }
}