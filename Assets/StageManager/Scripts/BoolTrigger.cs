using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolTrigger : MonoBehaviour {
  private const int FRAMES_TO_LIVE = 2;

  [SerializeField]
  private string _triggerString;

  [SerializeField]
  private Animator _targetAnimator;

  private int _frameCounter = 0;
  private bool _isTriggered = false;

  public void Trigger()
  {
    _targetAnimator.SetBool(_triggerString, true);
    _isTriggered = true;
    _frameCounter = 0;
  }

  void Update() {
    if(!_isTriggered)
    {
      return;
    }

    if (_frameCounter >= FRAMES_TO_LIVE)
    {
      _isTriggered = false;
      _targetAnimator.SetBool(_triggerString, false);
      return;
    }

    _frameCounter++;
	}
}
