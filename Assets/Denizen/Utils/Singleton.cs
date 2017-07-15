﻿using System;
using UnityEngine;

namespace Denizen.Utils
{
  public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
  {
    public static event Action InstanceInitialized;

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
        InstanceInitialized.Fire();
      }
      else
      {
        Debug.LogError(gameObject.name + " attempting to create duplicate instance of: " + typeof(T).ToString());
      }
    }
  }
}