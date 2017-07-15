using UnityEngine;

namespace Denizen.Input
{
  public abstract class DenizenInputWrapper : MonoBehaviour
  {
    public DenizenInputManager.StartTrackingObjectDelegate StartTrackingObjectCallback;
    public DenizenInputManager.WrapperInputEventDelegate InputEventCallback;

    public abstract bool SupportsHandAccessors();
    public abstract bool TryGetRightHand(out DenizenTrackedObject trackedObject);
    public abstract bool TryGetLeftHand(out DenizenTrackedObject trackedObject);
    public abstract void Connect();
    public abstract void Disconnect();
  }
}
