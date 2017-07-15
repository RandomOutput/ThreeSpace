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

public class CueSelector : StateMachineBehaviour {
  [SerializeField]
  private string SelectedItem;

  public string SelectedCueName
  {
    get
    {
      return SelectedItem;
    }
  }

  public Cue SelectedCue
  {
    get
    {
      SelectedItem = SelectedItem == null ? "" : SelectedItem;
      Cue cue;
      if(!Cue.TryGetCue(SelectedItem, out cue))
      {
        return null;
      }

      return cue;
    }
  }

  public int SelectedCueIndex
  {
    get
    {
      string[] cueNames = Cue.CueNames;


      for (int i = 0; i < cueNames.Length; i++)
      {
        if(cueNames[i] == SelectedItem)
        {
          return i;
        }
      }

      return -1;
    }

    set
    {
      SelectedItem = Cue.CueNames[value];
    }
  }

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    Cue cue = SelectedCue;
    if(cue != null)
    {
      cue.DoCueEntry();
    }
	}

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Cue cue = SelectedCue;
    if (cue != null)
    {
      cue.DoCueExit();
    }
  }
}
