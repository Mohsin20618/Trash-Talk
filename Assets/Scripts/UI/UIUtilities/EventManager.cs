using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventManager
{
    //public delegate void UpdateUIDelegate(string type);
    //public static event UpdateUIDelegate UpdateUI;
    public static Action<string> UpdateUI = null;
}
