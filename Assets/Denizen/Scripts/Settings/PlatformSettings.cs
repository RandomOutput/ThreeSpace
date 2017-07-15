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
