using System;
using UnityEngine;

namespace ThreeSpace {
  public class FocuserEvent : EventArgs {
    public readonly bool FocusEnabled;

    public FocuserEvent(bool focusEnabled)
    {
      FocusEnabled = focusEnabled;
    }
  }

  public interface IFocuser {
    event EventHandler<FocuserEvent> FocusEnabledChanged;

    GameObject FocusSource
    {
      get;
    }

    bool FocusEnabled
    {
      get;
    }
  }
}
