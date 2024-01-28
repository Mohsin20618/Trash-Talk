using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingLoader : MonoBehaviour
{
    public static WaitingLoader instance;
    public Transform loaderTransform;
    private Vector3 rotationEuler;

    void Awake()
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
        loaderTransform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (loaderTransform.gameObject.activeInHierarchy)
        {
            rotationEuler -= Vector3.forward * 180 * Time.deltaTime; //increment 30 degrees every second
            loaderTransform.rotation = Quaternion.Euler(rotationEuler);
        }
        else
        {
            rotationEuler -= Vector3.forward * 180 * Time.deltaTime; //increment 30 degrees every second
        }
    }

    internal void ShowHide(bool doShow = false)
    {
        gameObject.SetActive(doShow);
        loaderTransform.parent.gameObject.SetActive(doShow);
    }
}