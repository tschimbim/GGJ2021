using Photon.Pun;
using UnityEngine;

public class SingletonPUN<T> : MonoBehaviourPun where T : SingletonPUN<T>
{
    public static T instance { get; private set; } = default;

    protected SingletonPUN() { }

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
