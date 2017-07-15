using UnityEngine;
using Denizen.Utils;
using Denizen.Settings;

namespace Denizen
{
  public class DenizenCameraManager : Singleton<DenizenCameraManager>
  {
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _cameraCenterTransform;

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

      PlatformSettings settings = DenizenSettingsInjector.Instance.GetSettings<PlatformSettings>();

      if (settings.Platform == VRPlatform.STEAM_VR) {
        GameObject steamVRInstance = GameObject.Instantiate(settings.SteamVRCameraPrefab, transform);
        steamVRInstance.transform.localPosition = Vector3.zero;
        steamVRInstance.transform.localRotation = Quaternion.identity;
        newCamera = steamVRInstance.GetComponentInChildren<Camera>();
        overrideDefaultCamera = true;
      }

      if (overrideDefaultCamera && newCamera != null)
      {
        _cameraCenterTransform.parent = newCamera.transform;
        _cameraCenterTransform.localPosition = Vector3.zero;
        _cameraCenterTransform.localRotation = Quaternion.identity;
        _cameraCenterTransform.localScale = Vector3.one;

        Destroy(_camera.gameObject);
        _camera = newCamera;
      }
    }
  }
}
