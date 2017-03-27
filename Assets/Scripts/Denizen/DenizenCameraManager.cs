using UnityEngine;
using Denizen.Utils;
using Denizen.Settings;

namespace Denizen
{
  public class DenizenCameraManager : Singleton<DenizenCameraManager>
  {
    [SerializeField]
    private Camera _camera;

    public Camera Camera
    {
      get { return _camera; }
    }

    protected override void Awake()
    {
      base.Awake();
      SetupPlatformCamera();
    }

    private void SetupPlatformCamera()
    {
      bool overrideDefaultCamera = false;
      Camera newCamera = null;

      if(PlatformSettings.Instance.Platform == VRPlatform.STEAM_VR) {
        GameObject steamVRInstance = GameObject.Instantiate(PlatformSettings.Instance.SteamVRCameraPrefab, transform);
        steamVRInstance.transform.localPosition = Vector3.zero;
        steamVRInstance.transform.localRotation = Quaternion.identity;
        newCamera = steamVRInstance.GetComponentInChildren<Camera>();
        overrideDefaultCamera = true;
      }

      if (overrideDefaultCamera && newCamera != null)
      {
        Destroy(_camera.gameObject);
        _camera = newCamera;
      }
    }
  }
}
