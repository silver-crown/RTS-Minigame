using UnityEngine;
using System.Collections;
using HGC;
using System;
using JetBrains.Annotations;
using System.Diagnostics.CodeAnalysis;

public abstract class Singleton<T> : MonoBehaviour
    where T : Singleton<T>
{
    [SerializeField]
    protected bool _destroyOnDisable;

    [SerializeField]
    protected bool _isPersistent;

    [NonSerialized]
    private bool _isInitialised;

    private static Singleton<T> _instance;
    private static bool _applicationIsQuitting = false;
    private static object _lock = new object();

    public bool IsInitialised() { return _isInitialised; }

    #region StaticMethods
    public static bool HasInstance(bool error = false)
	{
#if UNITY_EDITOR
		if (error && _instance == null)
		{
			Debug.LogError("Singleton '" + typeof(T) + "' does not have an instance");
		}
#endif

		return _instance != null;
	}

    public static bool ValidateInstance()
    {
		return HasInstance();
    }

	// Can be overriden by other types of singletons
	protected virtual bool IsAvailable()
	{
		return true;
	}

	public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance (" + typeof(Singleton<T>) + ") was already destroyed on application quit, returning null.");
                    return null;
                }

                if (_instance != null)
                {
                    return _instance as T;
                }
                else
                {
                    _instance = FindObjectOfType(typeof(T)) as Singleton<T>;

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        singleton.name = typeof(T).ToString();

                        var component = singleton.AddComponent<T>() as Singleton<T>;
                        component._isPersistent = true;				

						DontDestroyOnLoad(singleton);

						_instance = component;
						return component as T;
                    }

					return _instance as T;
                }
            }
        }
    }
    #endregion


    #region InstanceMethods

    protected virtual void Awake()
    {
        if (HasInstance() && _instance != this)
        {
            Debug.LogError("A singleton (" + this + ") was a duplicate of", _instance);
            Debug.Break();
        }
        else
        {
            _instance = this;
            if (_isPersistent && transform.parent == null)
            {
                DontDestroyOnLoad(this);
            }
        }

        OnAwake();
    }

    protected abstract void OnAwake();
    protected abstract void OnShutdown();

    protected virtual void OnDisable()
    {
        if (_destroyOnDisable)
        {
            OnDestroy();
            Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            OnShutdown();
        }
    }

    protected virtual void OnApplicationQuit()
    {
        if (_instance == this)
        {
            Debug.Log("Cleaning up singleton on app quit: " + gameObject.name);
            _applicationIsQuitting = true;
            _instance = null;
            OnShutdown();
        }
    }
    #endregion
}
