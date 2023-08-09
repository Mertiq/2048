using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object LockObj = new();

    private static T instance;

    public static T Instance
    {
        get
        {
            lock (LockObj)
            {
                if (instance == null)
                    instance = FindObjectOfType<T>();

                return instance;
            }
        }
        internal set => instance = value;
    }
}