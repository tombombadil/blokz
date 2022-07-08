using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoreCont<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = (T)FindObjectOfType(typeof(T));

            if (_instance == null)
                Debug.LogWarning("[Singleton] Instance not found!");

            return _instance;
        }
    }

    public static C Cast<C>() where C : T
    {
        if (_instance == null)
            _instance = (T)FindObjectOfType(typeof(T));

        return (C)_instance;
    }
}