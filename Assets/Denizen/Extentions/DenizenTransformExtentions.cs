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
