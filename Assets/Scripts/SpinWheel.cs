using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheel : UIPanel
{

    public GameObject rotatingObject;
    public GameObject bonousScreen;
    public Text rewardText;
    public Button taptocollectBtn;
    public Button backBtn;

    public Button spinBtn;
    public List<RotationReward> rotationRewards;
    public float spinDuration =1f;
    private bool isSpinning = false;


    private void Awake()
    {
        spinBtn.onClick.AddListener(() => { StartCoroutine(SpinWheelCoroutine()); });
        backBtn.onClick.AddListener(() => { OnBackCallback(); });
        taptocollectBtn.onClick.AddListener(() => { OnRewardCollect(); });
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
    public void OnRewardCollect()
    {
        bonousScreen.SetActive(false);
    }
    IEnumerator SpinWheelCoroutine()
    {
        if (!isSpinning)
        {
            isSpinning = true;

            // Spin the wheel for 360 degrees in 2-3 seconds

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

            // Rotate to the selected target rotation and stop
            float stopDuration = 0.5f;
            float stopTime = 0f;

            while (stopTime < stopDuration)
            {
                float rotationAmount = Mathf.Lerp(0f, remainingRotation, stopTime / stopDuration);
                rotatingObject.transform.Rotate(Vector3.forward, rotationAmount);
                stopTime += Time.deltaTime;
                yield return null;
            }

            // Set the exact target rotation
            rotatingObject.transform.rotation = Quaternion.Euler(0f, 0f, selectedReward.targetRotation);

            // This is where you can do anything additional when the rotation is complete
            OnRotationComplete(selectedReward.reward);

            isSpinning = false;
        }
    }

    void OnRotationComplete(string reward)
    {
        // This method will be called when the rotation is complete
        // Provide the reward to the user
        Debug.Log("Reward: " + reward);
        rewardText.text = reward;
        bonousScreen.SetActive(true);
    }
}

    [Serializable]
    public class RotationReward
    {
        public float targetRotation;
        public string reward;
    }
