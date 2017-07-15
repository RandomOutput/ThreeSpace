using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CueSelector))]
public class CueSelectorEditor : Editor {

  private SerializedObject _cueObject;

  public override void OnInspectorGUI()
  {
    CueSelector selector = (CueSelector)serializedObject.targetObject;

    Cue cue = selector.SelectedCue;

    int newSelectedIndex = EditorGUILayout.Popup(selector.SelectedCueIndex, Cue.CueNames);

    bool validNewCue = newSelectedIndex != -1;

    if (!validNewCue)
    {
      EditorGUILayout.LabelField("Invalid Cue Selected: " + selector.SelectedCue);

      if (selector.SelectedCueName == "")
      {
        selector.SelectedCueIndex = 0;
      }

      return;
    }

    selector.SelectedCueIndex = newSelectedIndex;

    if (cue == null)
    {
      return;
    }

    _cueObject = new SerializedObject(cue);

    SerializedProperty prop = _cueObject.GetIterator();
    bool showChildren = true;
    while (prop.NextVisible(showChildren))
    {
      showChildren = false;

      if (prop.displayName == "Cue Name")
      {
        EditorGUILayout.LabelField(cue.CueName, EditorStyles.boldLabel);
        continue;
      }

      EditorGUILayout.PropertyField(prop, true);
    }

    _cueObject.ApplyModifiedProperties();
  }

}
