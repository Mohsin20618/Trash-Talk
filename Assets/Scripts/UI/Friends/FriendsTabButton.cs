using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FriendsTabButton : MonoBehaviour
{
    public GameObject hoverBG;
    public Text titleText;

    string activeColorCode = "#C88C06";

    public void SetActiveInactive(bool active)
    {
        hoverBG.SetActive(active);
        Color newColor = Color.white;

        if (active)
            ColorUtility.TryParseHtmlString(activeColorCode, out newColor);

        titleText.color = newColor;
    }
}
