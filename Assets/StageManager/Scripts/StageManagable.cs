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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

public class StageManagable : MonoBehaviour {
  public event Action StartEnterScene;
  public event Action CompleteEnterScene;
  public event Action StartExitScene;
  public event Action CompleteExitScene;

  public virtual void Enter(Animator stageManager)
  {
    StartEnter();
    CompleteEnter();
  }

  protected void StartEnter()
  {
    StartEnterScene.Fire();
  }

  protected void CompleteEnter()
  {
    CompleteEnterScene.Fire();
  }

  public virtual void Exit(Animator stageManager)
  {
    StartExit();
    CompleteExit();
  }

  protected void StartExit()
  {
    StartExitScene.Fire();
  }

  protected void CompleteExit()
  {
    CompleteExitScene.Fire();
  }
}
