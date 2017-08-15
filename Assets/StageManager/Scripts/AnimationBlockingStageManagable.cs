using System.Collections.Generic;
using UnityEngine;

namespace StageManager
{
  [RequireComponent(typeof(Animator))]
  public class AnimationBlockingStageManagable : StageManagable
  {
    private enum TransitionState
    {
      EXITED,
      ENTERING,
      ENTERED,
      EXITING
    }

    private const string BLOCKED_PARAMETER = "IsBlocked";
    private const string VISIBLE_PARAMETER = "IsVisible";

    private TransitionState transitionState;
    private Animator currentStageManager;

    public override void Enter(Animator stageManager)
    {
      if (transitionState == TransitionState.ENTERED ||
          transitionState == TransitionState.ENTERING)
      {
        return;
      }

      StartEnter();
      stageManager.SetBool(BLOCKED_PARAMETER, true);
      currentStageManager = stageManager;
    }

    public override void Exit(Animator stageManager)
    {
      if(transitionState == TransitionState.EXITED ||
         transitionState == TransitionState.EXITING)
      {
        return;
      }

      StartExit();
      stageManager.SetBool(BLOCKED_PARAMETER, true);
      currentStageManager = stageManager;
    }

    public void ANIM_EVENT_AnimationComplete()
    {
      if(transitionState == TransitionState.ENTERING)
      {
        transitionState = TransitionState.ENTERED;
      }
      else if(transitionState == TransitionState.EXITING)
      {
        transitionState = TransitionState.EXITED;
      }


    }
  }
}
