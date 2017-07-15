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

namespace ThreeSpace
{
  public class FocusManagerEvent : EventArgs
  {
    public readonly bool FocusEnabled;

    public FocusManagerEvent(bool focusEnabled)
    {
      FocusEnabled = focusEnabled;
    }
  }

  public class FocusManager : Singleton<FocusManager>
  {
    public delegate void FocusDelegate(IFocuser focuser);

    private struct FocusableDelegates
    {
      public readonly FocusDelegate FocusDelegate;
      public readonly FocusDelegate UnfocusDelegate;

      public FocusableDelegates(FocusDelegate focusDelegate, FocusDelegate unfocusDelegate)
      {
        FocusDelegate = focusDelegate;
        UnfocusDelegate = unfocusDelegate;
      }
    }

    public event EventHandler<FocusManagerEvent> OnFocusEnabledChanged;

    [SerializeField]
    private bool _focusEnabled;

    private Dictionary<IFocusable, FocusableDelegates> _registeredFocusables;

    public bool FocusEnabled
    {
      get { return _focusEnabled; }
      set
      {
        if(_focusEnabled == value)
        {
          return;
        }

        _focusEnabled = value;
        OnFocusEnabledChanged.Fire(this, new FocusManagerEvent(_focusEnabled));
      }
    }

    public void Register(IFocusable focusable, FocusDelegate focusDelegate, FocusDelegate unfocusDelegate)
    {
      if (_registeredFocusables == null)
      {
        _registeredFocusables = new Dictionary<IFocusable, FocusableDelegates>();
      }

      if(_registeredFocusables.ContainsKey(focusable))
      {
        Debug.LogError("Attempting to re-register focusable.");
        return;
      }

      _registeredFocusables.Add(focusable, new FocusableDelegates(focusDelegate, unfocusDelegate));
    }

    public void Deregister(IFocusable focusable)
    {
      if (_registeredFocusables == null)
      {
        Debug.LogError("Attempting to de-register a when no focusable are registered.");
        return;
      }

      if (!_registeredFocusables.ContainsKey(focusable))
      {
        Debug.LogError("Attempting to de-register a non-registered focusable.");
        return;
      }

      _registeredFocusables.Remove(focusable);
    }

    public bool TryFocus(IFocuser focuser, IFocusable focusable)
    {
      if(!_focusEnabled)
      {
        return false;
      }

      FocusableDelegates delegates;
      if(!_registeredFocusables.TryGetValue(focusable, out delegates))
      {
        return false;
      }

      delegates.FocusDelegate(focuser);
      return true;
    }

    public bool TryUnfocus(IFocuser focuser, IFocusable focusable)
    {
      FocusableDelegates delegates;
      if(!_registeredFocusables.TryGetValue(focusable, out delegates))
      {
        return false;
      }

      delegates.UnfocusDelegate(focuser);
      return true;
    }
  }
}
