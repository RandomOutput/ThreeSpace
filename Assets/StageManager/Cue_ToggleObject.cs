using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Cue : MonoBehaviour
{
  [SerializeField]
  private string _cueName;

  private string _lastCueName;

  private static Dictionary<string, Cue> Cues;

  public static string[] CueNames
  {
    get
    {
      if(Cues == null)
      {
        return new string[0];
      }

      string[] cueNames = new string[Cues.Keys.Count];
      Cues.Keys.CopyTo(cueNames, 0);
      return cueNames;
    }
  }

  public static bool TryGetCue(string cueName, out Cue cue)
  {
    if(Cues == null || cueName == null) {
      cue = null;
      return false;
    }

    if(!Cues.TryGetValue(cueName, out cue)) {
      return false;
    }

    return true;
  }

  public string CueName
  {
    get
    {
      return _cueName;
    }
  }

  private void OnEnable()
  {
    if(Cues == null)
    {
      Cues = new Dictionary<string, Cue>();
    }

    if(_cueName == null || _cueName == "")
    {
      return;
    }

    if(Cues.ContainsKey(_cueName)) { return; }
    RegisterCue();
  }

  private void RemoveCue()
  {
    if(Cues == null)
    {
      return;
    }

    if (_lastCueName != null && _lastCueName != "" && Cues.ContainsKey(_lastCueName))
    {
      if (Cues[_lastCueName] == this)
      {
        Cues.Remove(_lastCueName);
      }
    }

    if (Cues.ContainsValue(this))
    {
      foreach (Cue cue in Cues.Values)
      {
        if (cue == this)
        {
          Cues.Remove(cue._cueName);
          continue;
        }
      }
    }
  }

  private void RegisterCue()
  {
    if(Cues == null)
    {
      return;
    }

    if(_cueName == null || _cueName == "")
    {
      return;
    }

    RemoveCue();
    Cues.Add(_cueName, this);
  }

  private void OnDestroy()
  {
    RemoveCue();
  }

  private void Update()
  {
    if(Application.isPlaying)
    {
      return;
    }

    if(_lastCueName != _cueName)
    {
      RegisterCue();
    }

    _lastCueName = _cueName;
  }

  public virtual void DoCueEntry() { }
  public virtual void DoCueExit() { }
}

public class Cue_ToggleObject : Cue {

  [SerializeField]
  private StageManagable[] _stageManagableToEnterOnStart;

  [SerializeField]
  private StageManagable[] _stageManagableToExitOnCleanup;

  public override void DoCueEntry()
  {
    foreach (StageManagable stageManagable in _stageManagableToEnterOnStart)
    {
      stageManagable.Enter();
    }
  }

  public override void DoCueExit()
  {
    foreach (StageManagable stageManagable in _stageManagableToExitOnCleanup)
    {
      stageManagable.Exit();
    }
  }
}
