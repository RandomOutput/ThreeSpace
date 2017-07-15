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
