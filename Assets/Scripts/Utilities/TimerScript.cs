using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public Text countdownText;
    public Image countdownFillImage;
    private float totalTime;
    private float countdownTime;
    private Color startColor = Color.green; // Start color (green)
    private Color endColor = Color.red;     // End color (red)

    // Start is called before the first frame update
    void OnEnable()
    {
        totalTime = Global.turnMaxTime;
        countdownTime = Global.turnMaxTime;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTime = Mathf.Clamp(countdownTime - Time.deltaTime, 0f, 30f);

        ShowCounterText();
        UpdateImage();

        if (countdownTime <= 0)
        {
            GameplayManager.instance.OnTurnTimeUp();
            gameObject.SetActive(false);
        }
    }

    void ShowCounterText()
    {
        if (countdownText == null)
            return;
    }

    void UpdateImage()
    {
        if (countdownFillImage == null)
            return;

        countdownFillImage.fillAmount = countdownTime / totalTime;
        countdownFillImage.color = Color.Lerp(endColor, startColor, countdownTime / totalTime);

    }
}
