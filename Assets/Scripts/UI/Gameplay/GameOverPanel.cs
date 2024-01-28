using UnityEngine;

public class GameOverPanel : UIPanel
{
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void UpdateData(System.Action<object[]> callBack, params object[] parameters)
    {
        print(parameters[0]);
    }

    public void OnYesButton()
    {
        GameplayManager.instance.ResetGame();
        Hide();
    }

    public void OnNoButton()
    {
        Hide();
        UIEvents.HidePanel(Panel.GameplayPanel);
        UIEvents.ShowPanel(Panel.GameSelectPanel);
    }
}
