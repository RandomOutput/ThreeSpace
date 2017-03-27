using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

namespace Denizen.Settings
{
  [CreateAssetMenu(fileName = "PlatformSettings")]
  public class PlatformSettings : ScriptableSingleton<PlatformSettings>
  {
    public VRPlatform Platform
    {
      get { return _platform;  }
    }

    public GameObject SteamVRCameraPrefab
    {
      get { return _steamVRCameraPrefab; }
    }

    [Tooltip("Current platform to spawn correct prefabs and split code-paths for platform specific code.")]
    [SerializeField]
    private VRPlatform _platform;

    [Tooltip("Camera Prefab to be spawned if the current platform is SteamVR")]
    [SerializeField]
    private GameObject _steamVRCameraPrefab;
  }
}
