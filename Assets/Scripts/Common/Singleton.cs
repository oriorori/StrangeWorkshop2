using System;
using FishNet.Object;
using UnityEngine;

public abstract class Singleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                lock (_lock)
                {
                    _instance = FindObjectOfType<T>();

                    if (!_instance)
                    {
                        Debug.LogError($"No object of type {typeof(T)} found in scene.");
                    }
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Duplicate instance of {typeof(T).Name} found and destroyed.");
            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        _instance = null;
    }
}