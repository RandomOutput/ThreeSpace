using System;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

namespace ThreeSpace
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
      _registeredInteractables = new Dictionary<IInteractable, InteractableDelegates>();
    }

    public void Register(IInteractable interactable, InteractionDelegate startInteractionDelegate, InteractionDelegate endInteractionDelegate)
    {
      if (_registeredInteractables.ContainsKey(interactable))
      {
        Debug.LogError("Attempting to re-register interactable.");
        return;
      }

      _registeredInteractables.Add(interactable, new InteractableDelegates(startInteractionDelegate, endInteractionDelegate));
    }

    public void Deregister(IInteractable interactable)
    {
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
