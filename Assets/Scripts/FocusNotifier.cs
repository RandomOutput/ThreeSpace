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

    protected virtual void Start()
    {
      FocusManager.Instance.Register(this, Focus, Unfocus);
    }

    protected virtual void OnDestroy()
    {
      FocusManager.Instance.Deregister(this);
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
