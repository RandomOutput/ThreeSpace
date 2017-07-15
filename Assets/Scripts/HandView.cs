using System;
using UnityEngine;
using Denizen.Utils;
using Denizen.Input;

public class HandView : MonoBehaviour {
  [SerializeField]
  private Chirality _handChirality = Chirality.NONE;

  public event Action HandEnabled;
  public event Action HandDisabled;

  private bool _handEnabled = true;

  public Chirality HandChirality
  {
    get { return _handChirality; }
  }

  public bool IsHandEnabled
  {
    get { return _handEnabled; }
    set
    {
      if(_handEnabled == value)
      {
        return;
      }

      _handEnabled = value;

      foreach(Transform child in transform)
      {
        child.gameObject.SetActive(_handEnabled);
      }

      if (value)
      {
        HandEnabled.Fire();
      }
      else
      {
        HandDisabled.Fire();
      }
    }
  }

  private void Awake()
  {
    IsHandEnabled = false;
  }

  private void Update()
  {
    UpdateHandTracking();
    DebugInteractionRay();
  }

  private void UpdateHandTracking()
  {
    DenizenTrackedObject TrackedHand;
    if (!DenizenInputManager.Instance.TryGetChiralHand(_handChirality, out TrackedHand))
    {
      IsHandEnabled = false;
      return;
    }

    IsHandEnabled = TrackedHand.IsTracked;
    Vector3 position;
    Quaternion orientation;
    TrackedHand.TryGetPosition(out position);
    TrackedHand.TryGetOrientation(out orientation);
    transform.localPosition = position;
    transform.localRotation = orientation;
  }

  private void DebugInteractionRay()
  {
    DenizenTrackedObject TrackedHand;
    if (!DenizenInputManager.Instance.TryGetChiralHand(_handChirality, out TrackedHand))
    {
      return;
    }

    Ray interactionRay;
    if(!TrackedHand.TryGetInteractionRay(out interactionRay))
    {
      return;
    }

    Debug.DrawRay(interactionRay.origin, interactionRay.direction, Color.cyan);
  }


}
