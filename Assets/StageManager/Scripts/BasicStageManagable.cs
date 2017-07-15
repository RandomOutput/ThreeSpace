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
