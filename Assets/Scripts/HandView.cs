using UnityEngine;
using Denizen.Utils;
using Denizen.Input;

public class HandView : MonoBehaviour {
  [SerializeField]
  public Chirality _handChirality;

  private bool _handEnabled = true;

  public bool HandEnabled
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
    }
  }

  private void Awake()
  {
    HandEnabled = false;
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
      HandEnabled = false;
      return;
    }

    HandEnabled = TrackedHand.IsTracked;
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
