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
