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
  public static class DenizenTransformExtentions
  {
    public static Transform SetLocalX(this Transform t, float value)
    {
      Vector3 localPosition = t.localPosition;
      localPosition.x = value;
      t.localPosition = localPosition;
      return t;
    }

    public static Transform SetLocalY(this Transform t, float value)
    {
      Vector3 localPosition = t.localPosition;
      localPosition.y = value;
      t.localPosition = localPosition;
      return t;
    }

    public static Transform SetLocalZ(this Transform t, float value)
    {
      Vector3 localPosition = t.localPosition;
      localPosition.z = value;
      t.localPosition = localPosition;
      return t;
    }

    public static Transform SetX(this Transform t, float value)
    {
      Vector3 position = t.position;
      position.x = value;
      t.position = position;
      return t;
    }

    public static Transform SetY(this Transform t, float value)
    {
      Vector3 position = t.position;
      position.y = value;
      t.position = position;
      return t;
    }

    public static Transform SetZ(this Transform t, float value)
    {
      Vector3 position = t.position;
      position.z = value;
      t.position = position;
      return t;
    }

    public static Transform SetLocalXScale(this Transform t, float value)
    {
      Vector3 scale = t.localScale;
      scale.x = value;
      t.localScale = scale;
      return t;
    }

    public static Transform SetLocalYScale(this Transform t, float value)
    {
      Vector3 scale = t.localScale;
      scale.y = value;
      t.localScale = scale;
      return t;
    }

    public static Transform SetLocalZScale(this Transform t, float value)
    {
      Vector3 scale = t.localScale;
      scale.z = value;
      t.localScale = scale;
      return t;
    }
  }
}
