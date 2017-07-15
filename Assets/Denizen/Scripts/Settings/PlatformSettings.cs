using UnityEngine;

namespace Denizen.Settings
{
  [CreateAssetMenu(fileName = "PlatformSettings")]
  public class PlatformSettings : ScriptableObject
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
