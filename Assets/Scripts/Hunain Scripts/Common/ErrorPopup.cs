using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopup : MonoBehaviour
{
    public static ErrorPopup instance;
    public Text messageText;
    public Button btnOk;

    private System.Action callback;

    // Start is called before the first frame update
    private void Awake()
    {      
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {       
        btnOk.onClick.AddListener(() => OkButton());
    }

    public void ShowMessage(string message, System.Action action = null)
    {
        this.callback = action;
        messageText.text = message;
        gameObject.SetActive(true);
    }

    public void OkButton()
    {
        gameObject.SetActive(false);

        if (this.callback != null)
        {
            this.callback();
        }
    }
}