using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
{
    public static T instance { get; private set; } = default;

    protected SingletonBase() { }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarningFormat("Multiple singletons detected. Destroying second singleton instance with name {0}", gameObject.name);
            Destroy(this);
            return;
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
