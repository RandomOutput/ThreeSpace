// Copyright 2017 Daniel Plemmons

// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Denizen.Utils
{
  public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
  {
    public static event Action InstanceInitialized;

    public static List<Action> initializedActionQueue;

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
        fireAfterInitializedActions();
        InstanceInitialized.Fire();
      }
      else
      {
        Debug.LogError(gameObject.name + " attempting to create duplicate instance of: " + typeof(T).ToString());
      }
    }

    /// <summary>
    /// Adds an action to be performed after the instance is initialized, or
    /// immediately if the instance is already initialized.
    /// </summary>
    /// <returns>If the action was fired immediately</returns>
    public static bool AddAfterInitializedAction(Action action)
    {
      if(initializedActionQueue == null)
      {
        initializedActionQueue = new List<Action>();
      }

      if(Instance == null)
      {
        initializedActionQueue.Add(action);
        return false;
      }

      action();
      return true;
    }

    private static void fireAfterInitializedActions()
    {
      if(initializedActionQueue == null)
      {
        return;
      }

      for(int i=0;i<initializedActionQueue.Count;i++)
      {
        initializedActionQueue[i]();
      }

      initializedActionQueue.Clear();
    }
  }
}