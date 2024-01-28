using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CheckInternetConnection : Singleton<CheckInternetConnection>
{
    public GameObject popUp;
    //public Text messageTxt;

    private void Start()
    {
        CheckNetworkConnection();
    }

    public void CheckNetworkConnection()
    {
        StartCoroutine(CheckConnectivity());
    }

    IEnumerator CheckConnectivity()
    {
        
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("NetworkReachability.NotReachable = Not Reachable......");
            popUp.gameObject.SetActive(true);
        }
        else
        {
            UnityWebRequest request = new UnityWebRequest("https://google.com");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Not Connected......");
                popUp.gameObject.SetActive(true);
            }
        }

        yield return new WaitForSeconds(2f);
        CheckNetworkConnection(); //Repeat
    }
}