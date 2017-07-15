using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStageManagable : StageManagable{
  public void Awake()
  {
    gameObject.SetActive(false);
  }

  public override void Enter()
  {
    StartEnter();
    gameObject.SetActive(true);
    CompleteEnter();
  }

  public override void Exit()
  {

    StartExit();
    gameObject.SetActive(false);
    CompleteExit();
  }
}
