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
