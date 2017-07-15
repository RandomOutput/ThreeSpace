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
  public class FocusNotifier : MonoBehaviour, IFocusable
  {
    public event Action Focused;
    public event Action Unfocused;

    private bool _isFocused = false;
    private IFocuser _focuser;

    public IFocuser Focuser
    {
      get { return _focuser; }
    }

    public bool IsFocused
    {
      get { return _isFocused; }
    }

    public GameObject ParentObject
    {
      get { return gameObject; }
    }

    protected virtual void OnEnable()
    {
      FocusManager.AddAfterInitializedAction(() => FocusManager.Instance.Register(this, Focus, Unfocus));
    }

    protected virtual void OnDisable()
    {
      FocusManager.AddAfterInitializedAction(() => FocusManager.Instance.Deregister(this));
    }

    private void Focus(IFocuser focuser)
    {
      _isFocused = true;
      _focuser = focuser;
      _focuser.FocusEnabledChanged += _focuser_FocusEnabledChanged;
      Focused.Fire();
    }

    private void Unfocus(IFocuser focuser)
    {
      _isFocused = false;
      _focuser.FocusEnabledChanged -= _focuser_FocusEnabledChanged;
      _focuser = null;
      Unfocused.Fire();
    }

    public bool TryFocus(IFocuser focuser)
    {
      if(focuser == null ||
         !focuser.FocusEnabled ||
         _isFocused)
      {
        return false;
      }

      return FocusManager.Instance.TryFocus(focuser, this);
    }

    private void _focuser_FocusEnabledChanged(object sender, FocuserEvent e)
    {
      IFocuser changedFocuser = (IFocuser)sender;

      if(changedFocuser != _focuser)
      {
        return;
      }

      if(!e.FocusEnabled)
      {
        TryUnfocus(changedFocuser);
      }
    }

    public bool TryUnfocus(IFocuser focuser)
    {
      if(_focuser == null ||
         !_isFocused ||
         _focuser != focuser)
      {
        return false;
      }

      return FocusManager.Instance.TryUnfocus(focuser, this);
    }
  }
}
