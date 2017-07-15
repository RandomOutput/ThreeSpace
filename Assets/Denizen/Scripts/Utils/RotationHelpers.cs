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

namespace Denizen.Utils
{
  public static class RotationHelpers
  {
    public static Quaternion AxialFromToRotation(Vector3 from, Vector3 to, Vector3 axis)
    {
      //Angle
      Vector3 planarFrom = Vector3.ProjectOnPlane(from.normalized, axis.normalized);
      Vector3 planarTo = Vector3.ProjectOnPlane(to.normalized, axis.normalized);
      float angle = Vector3.Angle(planarFrom.normalized, planarTo.normalized);

      //Sign of the angle
      Vector3 cross = Vector3.Cross(planarFrom, planarTo);
      float dot = Vector3.Dot(cross.normalized, axis.normalized);
      int sign = dot >= 0.0f ? 1 : -1;
      angle *= sign;

      // Create rotation
      Quaternion rotation = Quaternion.AngleAxis(angle, axis);
      return rotation;
    }
  }
}
