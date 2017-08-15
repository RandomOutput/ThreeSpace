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
using LineDrawing;

namespace Denizen.Input.Interactions
{
  public class FocusChangedEvent : EventArgs
  {
    public readonly IFocusable FocusedTarget;

    public FocusChangedEvent(IFocusable focusedTarget)
    {
      FocusedTarget = focusedTarget;
    }
  }

  public class InteractionChangedEvent : EventArgs
  {
    public readonly IInteractable InteractionTarget;

    public bool IsInteracting
    {
      get { return InteractionTarget != null; }
    }

    public InteractionChangedEvent(IInteractable interactionTarget)
    {
      InteractionTarget = interactionTarget;
    }
  }

  [RequireComponent(typeof(HandView))]
  public class HandInteractionController : MonoBehaviour, IFocuser, IInteractor
  {
    public event EventHandler<FocuserEvent> FocusEnabledChanged;
    public event EventHandler<InteractorEvent> InteractionEnabledChanged;
    public event EventHandler<FocusChangedEvent> FocusChanged;
    public event EventHandler<InteractionChangedEvent> InteractionChanged;

    [SerializeField]
    private bool _focusEnabled;

    [SerializeField]
    private bool _interactionEnabled;

    [SerializeField]
    private LayerMask _interactableLayer;

    [SerializeField]
    private Material _interactionLineMaterial = null;

    [SerializeField]
    private float _maxInteractionRayLength = 0;

    [SerializeField]
    private Transform _interactionRayEndpoint = null;

    private HandView _handView;
    private DenizenTrackedObject _trackedHand;

    private IFocusable _focusedTarget;
    private IInteractable _interactionTarget;
    private Vector3 _interactionPoint;
    private LineDrawing.Line _interactionRay;
    private float _targetDistanceAtInteractionStart = 0;
    private RaycastHit _lastInteractionRaycast;
    private bool _lastRaycastHasHit;

    public bool FocusEnabled
    {
      get { return _focusEnabled; }

      set
      {
        if(_focusEnabled != value)
        {
          return;
        }

        _focusEnabled = value;
        FocusEnabledChanged.Fire(this, new FocuserEvent(_focusEnabled));
      }
    }

    public GameObject FocusSource
    {
      get { return gameObject; }
    }

    public bool InteractionEnabled
    {
      get { return _interactionEnabled; }

      set
      {
        if (_interactionEnabled != value)
        {
          return;
        }

        _interactionEnabled = value;
        InteractionEnabledChanged.Fire(this, new InteractorEvent(_interactionEnabled));
      }
    }

    public bool IsInteracting
    {
      get { return _interactionTarget != null;  }
    }

    public GameObject InteractionSource
    {
      get { return gameObject; }
    }

    public Vector3 InteractionPoint
    {
      get { return _interactionPoint; }
    }

    private float interactionDistance
    {
      get
      {
        return IsInteracting ? _targetDistanceAtInteractionStart : _lastInteractionRaycast.distance;
      }
    }

    private void enableInteraction()
    {
      registerHandInputEvents();
      SetInteractionRayActive(true);
    }

    private void disableInteraction()
    {
      deregisterHandInputEvents();
      SetInteractionRayActive(false);
    }

    private void SetInteractionRayActive(bool active)
    {
      _interactionRay.gameObject.SetActive(active);
      _interactionRayEndpoint.gameObject.SetActive(active);
    }

    private void registerHandInputEvents()
    {
      if (!DenizenInputManager.Instance.TryGetChiralHand(_handView.HandChirality, out _trackedHand))
      {
        return;
      }

      if (_trackedHand == null)
      {
        return;
      }

      _trackedHand.PrimaryPressed += TrackedHand_PrimaryPressed;
      _trackedHand.PrimaryReleased += TrackedHand_PrimaryReleased;
    }

    private void deregisterHandInputEvents()
    {
      if(_trackedHand == null)
      {
        return;
      }

      _trackedHand.PrimaryPressed -= TrackedHand_PrimaryPressed;
      _trackedHand.PrimaryReleased -= TrackedHand_PrimaryReleased;

    }

    private void TrackedHand_PrimaryPressed(object sender, DenizenTrackedObjectEventData e)
    {
      if (_focusedTarget == null)
      {
        return;
      }

      IInteractable interactable = _focusedTarget.ParentObject.GetComponent<IInteractable>();

      if(interactable == null)
      {
        return;
      }

      if (!interactable.TryStartInteraction(this))
      {
        return;
      }

      _targetDistanceAtInteractionStart = interactionDistance;
      _interactionTarget = interactable;
      InteractionChanged.Fire(this, new InteractionChangedEvent(_interactionTarget));
    }

    private void TrackedHand_PrimaryReleased(object sender, DenizenTrackedObjectEventData e)
    {
      if(_interactionTarget == null)
      {
        return;
      }

      if(!_interactionTarget.TryStopInteraction(this))
      {
        return;
      }

      _interactionTarget = null;
      InteractionChanged.Fire(this, new InteractionChangedEvent(null));
    }

    protected virtual void Awake()
    {
      CreateInteractionRay();
    }

    protected virtual void Start()
    {
      _handView = GetComponent<HandView>();
      _handView.HandEnabled += enableInteraction;
      _handView.HandDisabled += disableInteraction;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
      if (!_handView.IsHandEnabled)
      {
        return;
      }

      DenizenTrackedObject TrackedHand;
      if (!DenizenInputManager.Instance.TryGetChiralHand(_handView.HandChirality, out TrackedHand))
      {
        return;
      }

      Ray interactionRay;
      if (!TrackedHand.TryGetInteractionRay(out interactionRay))
      {
        return;
      }

      _lastRaycastHasHit = Physics.Raycast(interactionRay, out _lastInteractionRaycast, _maxInteractionRayLength, _interactableLayer);

      if(!_lastRaycastHasHit)
      {
        _lastInteractionRaycast.distance = _maxInteractionRayLength;
      }

      UpdateInteractionRay();
      UpdateFocusedObject();
      UpdateInteractionPoint();
    }

    private void UpdateFocusedObject()
    {
      IFocusable focusTarget = _lastRaycastHasHit ? _lastInteractionRaycast.collider.gameObject.GetComponent<FocusNotifier>() : null;
      bool hasFocusTarget = focusTarget != null;

      if(hasFocusTarget)
      {
        if(_focusedTarget == null)
        {
          FocusOn(focusTarget);
        }
        else if(focusTarget == _focusedTarget)
        {

          return;
        }
        else
        {
          ClearFocus();
          FocusOn(focusTarget);
        }
      }
      else if(_focusedTarget != null)
      {
        ClearFocus();
      }
    }

    private void UpdateInteractionPoint()
    {
      _interactionPoint = transform.position + transform.forward * interactionDistance;
    }

    private void FocusOn(IFocusable target)
    {
      if(!target.TryFocus(this))
      {
        return;
      }
      _focusedTarget = target;
      FocusChanged.Fire(this, new FocusChangedEvent(target));
    }

    private void ClearFocus()
    {
      if(_focusedTarget == null)
      {
        return;
      }

      if(!_focusedTarget.TryUnfocus(this))
      {
        return;
      }
      _focusedTarget = null;
      FocusChanged.Fire(this, new FocusChangedEvent(null));
    }

    private void CreateInteractionRay()
    {
      _interactionRay = new Line(2, 3, 0.005f);
      _interactionRay.gameObject.GetComponent<Renderer>().material = _interactionLineMaterial;
      _interactionRay.gameObject.SetActive(false);
    }

    private void UpdateInteractionRay()
    {
      DenizenTrackedObject TrackedHand;
      if (!DenizenInputManager.Instance.TryGetChiralHand(_handView.HandChirality, out TrackedHand))
      {
        return;
      }

      Ray interactionRay;
      if (!TrackedHand.TryGetInteractionRay(out interactionRay))
      {
        return;
      }

      Vector3[] verts = _interactionRay.Verticies;
      Vector3 hitPosition = interactionRay.origin + (interactionRay.direction * interactionDistance);
      verts[0] = interactionRay.origin;
      verts[1] = hitPosition;
      _interactionRay.SetVerticies(verts);

      if (_interactionRayEndpoint != null)
      {
        _interactionRayEndpoint.position = hitPosition;
      }
    }
  }
}
