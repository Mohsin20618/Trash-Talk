using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : UIPanel
{
    public RewardPanel rewardPanel;
    public GameObject rotatingObject;
    //public GameObject bonousScreen;
    //public Text rewardText;
    //public Button taptocollectBtn;
    public Button backBtn;

    public Button spinBtn;
    public List<RotationReward> rotationRewards;
    public float spinDuration =1f;
    private bool isSpinning = false;
    private string rewardCoin;

    private void Awake()
    {
        spinBtn.onClick.AddListener(() => { StartCoroutine(SpinWheelCoroutine()); });
        backBtn.onClick.AddListener(() => { OnBackCallback(); });
        //taptocollectBtn.onClick.AddListener(() => { OnRewardCollect(); });
    }
    private void OnEnable()
    {
        spinBtn.interactable = true;
    }
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(Action<object[]> callBack, params object[] parameters)
    {
    }

    private void OnBackCallback() 
    {
        UIEvents.ShowPanel(Panel.TabPanels);
        UIEvents.HidePanel(Panel.SpinWheel);
    }
    //public void OnRewardCollect()
    //{
    //    bonousScreen.SetActive(false);
    //    Hide();
    //    UIEvents.ShowPanel(Panel.TabPanels);
    //}
    IEnumerator SpinWheelCoroutine()
    {
        if (!isSpinning)
        {
            isSpinning = true;

            // Spin the wheel for 360 degrees in 2-3 seconds
            float spinDuration = UnityEngine.Random.Range(2f, 3f);
            float elapsedTime = 0f;

            while (elapsedTime < spinDuration)
            {
                float rotationAmount = Mathf.Lerp(0f, 360f, elapsedTime / spinDuration);
                rotatingObject.transform.Rotate(Vector3.forward, rotationAmount * Time.deltaTime / Time.fixedDeltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Select a random target rotation and associated reward
            int randomIndex = UnityEngine.Random.Range(0, rotationRewards.Count);
            RotationReward selectedReward = rotationRewards[randomIndex];

            // Determine the remaining rotation needed to reach the target
            float remainingRotation = selectedReward.targetRotation - rotatingObject.transform.rotation.eulerAngles.z;

            // Rotate to the selected target rotation and stop with a gradual easing-out effect
            LeanTween.rotateZ(rotatingObject, rotatingObject.transform.rotation.eulerAngles.z + remainingRotation, 1.5f)
                .setEaseOutExpo()  // Experiment with different easing functions
                .setOnComplete(() => OnRotationComplete(selectedReward.reward));

            isSpinning = false;
        }
    }

    void OnRotationComplete(string reward)
    {
        // This method will be called when the rotation is complete
        // Provide the reward to the user
        Debug.Log("Reward: " + reward);
        //rewardText.text = reward;
        rewardCoin = reward;
        spinBtn.interactable = false;
        Invoke("ShowBonousScreen", 1f);
    }
    private void ShowBonousScreen()
    {
        rewardPanel.SetCoinText(rewardCoin);
        CountDownTimer.instance.UpdateSpinTimer(rewardCoin);
    }
}

[Serializable]
    public class RotationReward
    {
        public float targetRotation;
        public string reward;
    }
