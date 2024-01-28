/*
 * Script Ownership Information:
 * Author: Muhammad Hunain
 *
 * Description: 
 * To use this generic Singleton class, you can create a new class and inherit from Singleton<T>, where T is the derived class itself. 
 * Using a static instance ensures that only one instance of the class exists throughout your Unity project, and you can access it easily from anywhere in your code.
 *
 * Legal Notice:
* This script is the intellectual property of Cubix.inc. Any unauthorized use,
 * reproduction, or distribution is strictly prohibited without prior written permission.
 */

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public bool shouldPersistOnLoad = true;

    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>(true);

                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            if (shouldPersistOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else if (_instance != this)
        {
            if (shouldPersistOnLoad)
                Destroy(gameObject);
        }
    }
}