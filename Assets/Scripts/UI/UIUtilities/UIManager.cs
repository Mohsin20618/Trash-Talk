using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Dictionary<string, UIPanel> panels = new Dictionary<string, UIPanel>();

    public void RegisterPanel(string panelName, UIPanel panel)
    {
        panels[panelName] = panel;
    }

    private void OnEnable()
    {
        UIEvents.ShowPanelRequested += ShowPanel;
        UIEvents.HidePanelRequested += HidePanel;
        UIEvents.UpdateDataRequest += UpdatePanelData;

    }

    private void OnDisable()
    {
        UIEvents.ShowPanelRequested -= ShowPanel;
        UIEvents.HidePanelRequested -= HidePanel;
        UIEvents.UpdateDataRequest -= UpdatePanelData;

    }

    public void ShowPanel(string panelName)
    {
        if(panels.TryGetValue(panelName,out UIPanel panel))
        {
            panel.Show();
        }
    }

    public void HidePanel(string panelName)
    {
        if (panels.TryGetValue(panelName, out UIPanel panel))
        {
            panel.Hide();
        }
    }

    public void UpdatePanelData(string panelName, System.Action<object[]> callBack, params object[] parameters)
    {
        if (panels.TryGetValue(panelName, out UIPanel panel))
        {
            panel.UpdateData(callBack,parameters);
        }
    }
}
