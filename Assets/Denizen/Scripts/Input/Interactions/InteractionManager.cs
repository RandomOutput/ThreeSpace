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
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

namespace Denizen.Input.Interactions
{
  public class InteractionManagerEvent : EventArgs
  {
    public readonly bool InteractionEnabled;

    public InteractionManagerEvent(bool interactionEnabled)
    {
      InteractionEnabled = interactionEnabled;
    }
  }

  public class InteractionManager : Singleton<InteractionManager>
  {
    public delegate void InteractionDelegate(IInteractor interactor);

    private struct InteractableDelegates
    {
      public readonly InteractionDelegate StartInteractionDelegate;
      public readonly InteractionDelegate EndInteractionDelegate;

      public InteractableDelegates(InteractionDelegate startInteractionDelegate, InteractionDelegate endInteractionDelegate)
      {
        StartInteractionDelegate = startInteractionDelegate;
        EndInteractionDelegate = endInteractionDelegate;
      }
    }

    public event EventHandler<InteractionManagerEvent> OnInteractionEnabledChanged;

    [SerializeField]
    private bool _interactionEnabled;

    private Dictionary<IInteractable, InteractableDelegates> _registeredInteractables;

    public bool InteractionEnabled
    {
      get { return _interactionEnabled; }
      set
      {
        if (_interactionEnabled == value)
        {
          return;
        }

        _interactionEnabled = value;
        OnInteractionEnabledChanged.Fire(this, new InteractionManagerEvent(_interactionEnabled));
      }
    }

    protected override void Awake()
    {
      base.Awake();
    }

    public void Register(IInteractable interactable, InteractionDelegate startInteractionDelegate, InteractionDelegate endInteractionDelegate)
    {
      if(_registeredInteractables == null)
      {
        _registeredInteractables = new Dictionary<IInteractable, InteractableDelegates>();
      }

      if (_registeredInteractables.ContainsKey(interactable))
      {
        Debug.LogError("Attempting to re-register interactable.");
        return;
      }

      _registeredInteractables.Add(interactable, new InteractableDelegates(startInteractionDelegate, endInteractionDelegate));
    }

    public void Deregister(IInteractable interactable)
    {
      if(_registeredInteractables == null)
      {
        Debug.LogError("Attempting to de-register a when no interactables are registered.");
        return;
      }

      if (!_registeredInteractables.ContainsKey(interactable))
      {
        Debug.LogError("Attempting to de-register a non-registered interactable.");
        return;
      }

      _registeredInteractables.Remove(interactable);
    }

    public bool TryStartInteraction(IInteractor interactor, IInteractable interactable)
    {
      if(!_interactionEnabled)
      {
        return false;
      }

      InteractableDelegates delegates;
      if(!_registeredInteractables.TryGetValue(interactable, out delegates))
      {
        return false;
      }

      delegates.StartInteractionDelegate(interactor);
      return true;
    }

    public bool TryEndInteraction(IInteractor interactor, IInteractable interactable)
    {
      InteractableDelegates delegates;
      if (!_registeredInteractables.TryGetValue(interactable, out delegates))
      {
        return false;
      }

      delegates.EndInteractionDelegate(interactor);
      return true;
    }
  }
}
