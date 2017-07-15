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

  public virtual void Enter()
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

  public virtual void Exit()
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
