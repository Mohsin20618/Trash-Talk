using UnityEngine;

public class PanelsRegister : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private UIPanel[] panels;

    private void Start()
    {
        RegisterUIPanels();
    }

    private void RegisterUIPanels()
    {
        foreach(UIPanel panel in panels)
        {
            uiManager.RegisterPanel(panel.GetType().Name, panel);
        }

    }
}
