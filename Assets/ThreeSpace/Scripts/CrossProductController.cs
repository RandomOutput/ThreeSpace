using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeSpace
{
  [ExecuteInEditMode]
  public class CrossProductController : MonoBehaviour
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
      Vector3 crossProduct = Vector3.Cross(vecA.Vector, vecB.Vector);
      vecC.SetVector(vecA.Origin, vecA.Origin + crossProduct);
    }
  }
}
