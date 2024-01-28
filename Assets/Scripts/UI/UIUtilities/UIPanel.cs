using UnityEngine;

public abstract class UIPanel : MonoBehaviour
{
    public abstract void Show();
    public abstract void Hide();
    public abstract void UpdateData(System.Action<object[]> callBack, params object[] parameters);
}
