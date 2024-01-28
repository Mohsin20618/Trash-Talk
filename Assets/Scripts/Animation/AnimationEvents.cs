using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    Action<object[]> callBack;

    public void SetCallBack(Action<object[]> callBack)
    {
        this.callBack = callBack;
    }

    public void OnCardCoverScreen()
    {
        this.callBack(null);
    }

    public void OnCardIntroEnd()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
