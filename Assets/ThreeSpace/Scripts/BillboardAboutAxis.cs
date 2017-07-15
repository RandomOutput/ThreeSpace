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
using Denizen.Utils;

namespace ThreeSpace
{
  [ExecuteInEditMode]
  public class BillboardAboutAxis : MonoBehaviour
  {
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _worldAxis;

    [SerializeField]
    private bool _invertZFacing;

    // Update is called once per frame
    void Update()
    {
      if(_target == null)
      {
        return;
      }

      Vector3 toTarget = _target.position - transform.position;

      // Face world axis
      transform.up = _worldAxis;

      Vector3 vectorToFaceTarget = _invertZFacing ? -transform.forward : transform.forward;

      // Rotate about world axis
      Quaternion rotation = RotationHelpers.AxialFromToRotation(vectorToFaceTarget, toTarget, _worldAxis);
      transform.rotation = rotation * transform.rotation;
    }
  }
}
