using System;
using UnityEngine;

namespace ThreeSpace
{
  public class InteractorEvent : EventArgs
  {
    public readonly bool InteractionEnabled;

    public InteractorEvent(bool focusEnabled)
    {
      InteractionEnabled = focusEnabled;
    }
  }

  public interface IInteractor
  {
    event EventHandler<InteractorEvent> InteractionEnabledChanged;

    GameObject InteractionSource
    {
      get;
    }

    bool InteractionEnabled
    {
      get;
    }

    bool IsInteracting
    {
      get;
    }

    Vector3 InteractionPoint
    {
      get;
    }
  }
}
