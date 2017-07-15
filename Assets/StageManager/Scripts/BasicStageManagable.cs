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

public class BasicStageManagable : StageManagable{
  private bool hasInitialized = false;

  protected virtual void Awake()
  {
    Initialize();
  }

  protected virtual void Initialize()
  {
    if(hasInitialized)
    {
      return;
    }

    hasInitialized = true;
    gameObject.SetActive(false);
  }

  public override void Enter()
  {
    Initialize();
    StartEnter();
    gameObject.SetActive(true);
    CompleteEnter();
  }

  public override void Exit()
  {
    Initialize();
    StartExit();
    gameObject.SetActive(false);
    CompleteExit();
  }
}
