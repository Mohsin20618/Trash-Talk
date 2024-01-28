using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenTimeout : MonoBehaviour
{
    public Text BottomLoadingText; // if needed to be dynamically changed

    private void OnEnable()
    {
        Debug.LogError("OnEnable");
        StartCoroutine(Timer());
    }

    private IEnumerator Timer(string msg="")
    {
        BottomLoadingText.text = msg;
        yield return new WaitForSeconds(20f);
        Debug.LogError("waiting to turn off");
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        StopCoroutine(Timer());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
