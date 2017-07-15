using System;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

namespace Denizen {
  public class DenizenSettingsInjector :  Singleton<DenizenSettingsInjector> {
    [SerializeField]
    private ScriptableObject[] _settings;

    private Dictionary<Type, ScriptableObject> _settingsDictionary;

    public T GetSettings<T>() where T : ScriptableObject
    {
      ScriptableObject settingsFile;
      if (!_settingsDictionary.TryGetValue(typeof(T), out settingsFile))
      {
        return null;
      }
      T settingsFileCast = (T)settingsFile;
      return settingsFileCast;
    }

    protected override void Awake()
    {
      base.Awake();
      PopulateSettingsDictionary();
    }

    private void PopulateSettingsDictionary()
    {
      _settingsDictionary = new Dictionary<Type, ScriptableObject>(_settings.Length);
      foreach(ScriptableObject scriptableObject in _settings)
      {
        _settingsDictionary.Add(scriptableObject.GetType(), scriptableObject);
      }
    }
  }
}
