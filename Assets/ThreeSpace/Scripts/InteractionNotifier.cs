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
using Denizen.Utils;

namespace ThreeSpace
{
  [RequireComponent(typeof(FocusNotifier))]
  public class InteractionNotifier : MonoBehaviour, IInteractable
  {
    public event Action InteractionStart;
    public event Action InteractionEnd;

    private bool _isInteractedWith = false;
    private IInteractor _interactor;

    public bool IsInteractedWith
    {
      get { return _isInteractedWith; }
    }

    public IInteractor Interactor
    {
      get { return _interactor; }
    }

    public GameObject ParentObject
    {
      get { return gameObject; }
    }

    protected virtual void OnEnable()
    {
      InteractionManager.AddAfterInitializedAction(() => InteractionManager.Instance.Register(this, StartInteraction, EndInteraction));
    }

    protected virtual void OnDisable()
    {
        InteractionManager.AddAfterInitializedAction(() => InteractionManager.Instance.Deregister(this));
    }

    private void StartInteraction(IInteractor interactor)
    {
      _isInteractedWith = true;
      _interactor = interactor;
      _interactor.InteractionEnabledChanged += _interactor_InteractionEnabledChanged;
      InteractionStart.Fire();
    }

    private void EndInteraction(IInteractor interactor)
    {
      _isInteractedWith = false;
      _interactor = null;
      InteractionEnd.Fire();
    }

    public bool TryStartInteraction(IInteractor interactor)
    {
      if (interactor == null ||
         !interactor.InteractionEnabled ||
         _isInteractedWith)
      {
        return false;
      }

      return InteractionManager.Instance.TryStartInteraction(interactor, this);
    }

    private void _interactor_InteractionEnabledChanged(object sender, InteractorEvent e)
    {
      IInteractor changedInteractor = (IInteractor)sender;

      if (changedInteractor != _interactor)
      {
        return;
      }

      if (!e.InteractionEnabled)
      {
        TryStopInteraction(changedInteractor);
      }
    }

    public bool TryStopInteraction(IInteractor interactor)
    {
      if (_interactor == null ||
         !_isInteractedWith ||
         _interactor != interactor)
      {
        return false;
      }

      return InteractionManager.Instance.TryEndInteraction(interactor, this);
    }
  }
}
