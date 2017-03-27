using UnityEngine;

namespace Denizen.Utils
{
  public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
  {
    public static T Instance
    {
      get
      {
        return _instance;
      }
    }

    private static T _instance;

    protected virtual void OnEnable()
    {
      if(_instance == null)
      {
        _instance = (T)this;
      }
    }
  }

  public class Singleton<T> : MonoBehaviour where T : Singleton<T>
  {

    public static T Instance
    {
      get
      {
        return _instance;
      }
    }

    private static T _instance;

    protected virtual void Awake()
    {
      if (_instance == null)
      {
        _instance = (T)this;
      }
      else
      {
        Debug.LogError(gameObject.name + " attempting to create duplicate instance of: " + typeof(T).ToString());
      }
    }
  }
}