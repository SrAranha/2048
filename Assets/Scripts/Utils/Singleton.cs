using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;
    [SerializeField] private bool destroyOnLoad = true;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<T>();
            Debug.Log("Creating instance of " + instance);
        }
        else
        {
            Debug.Log("Deleting instance of " + instance);
            Destroy(gameObject);
        }
        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}