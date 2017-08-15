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
using UnityEditor;

namespace StageManager
{
  [CustomEditor(typeof(CueSelector))]
  public class CueSelectorEditor : Editor
  {

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
}
