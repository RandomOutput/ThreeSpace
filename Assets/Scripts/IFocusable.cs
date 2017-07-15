using System;
using UnityEngine;

namespace ThreeSpace
{
  public interface IFocusable
  {
    event Action Focused;
    event Action Unfocused;

    GameObject ParentObject
    {
      get;
    }

    bool IsFocused
    {
      get;
    }

    IFocuser Focuser
    {
      get;
    }

    bool TryFocus(IFocuser focuser);
    bool TryUnfocus(IFocuser focuser);
  }
}
