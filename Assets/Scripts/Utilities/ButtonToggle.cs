using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    public GameObject onObject;
    public GameObject offObject;

    System.Action<bool> onToggle;

    bool isOn = true;

    //public void SetToggle(bool on, System.Action<bool> onToggle)
     public void SetToggle(bool on)
    {
      //  this.onToggle = onToggle;
        isOn = on;
        UpdateUI(on);
    }

    public void OnPressToggle()
    {
        print("pressed");
        isOn = !isOn;
        UpdateUI(isOn);

        if(this.onToggle != null)
            this.onToggle(isOn);
    }

    public void UpdateUI(bool on)
    {
        onObject.SetActive(on);
        offObject.SetActive(!on);
    }
}