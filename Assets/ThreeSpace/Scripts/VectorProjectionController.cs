using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeSpace
{
  [ExecuteInEditMode]
  public class VectorProjectionController : MonoBehaviour
  {
    [SerializeField]
    private VectorView vecA;

    [SerializeField]
    private VectorView vecB;

    [SerializeField]
    private VectorView vecC;

    // Update is called once per frame
    void Update()
    {
      float dot = Vector3.Dot(vecA.Direction, vecB.Vector);
      vecC.EndPoint = vecA.Origin + vecA.Direction * dot;
    }
  }
}
