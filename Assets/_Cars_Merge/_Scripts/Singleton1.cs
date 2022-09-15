using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance;
    public static T instance => Instance;
   
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this) 
        {
            DestroyImmediate(this);
        }
    }
}

public abstract class SingletonInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T Instance;
    public static T instance => Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        
    }

}
