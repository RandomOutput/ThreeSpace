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
