public static class UIEvents
{
    public static event System.Action<string> ShowPanelRequested;
    public static event System.Action<string> HidePanelRequested;
    public static event System.Action<string, System.Action<object[]>, object[]> UpdateDataRequest;


    public static void ShowPanel(Panel panelName)
    {
        ShowPanelRequested?.Invoke(panelName.ToString());
    }

    public static void HidePanel(Panel panelName)
    {
        HidePanelRequested?.Invoke(panelName.ToString());
    }

    public static void UpdateData(Panel panelName, System.Action<object[]> callBack,params object [] parameters)
    {
        UpdateDataRequest?.Invoke(panelName.ToString(), callBack, parameters);
    }
}
