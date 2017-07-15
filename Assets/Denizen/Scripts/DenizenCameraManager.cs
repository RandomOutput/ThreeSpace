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
