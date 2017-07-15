using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeSpace {
  [ExecuteInEditMode]
  public class DotProductController : MonoBehaviour {

    [SerializeField]
    private VectorView vecA;

    [SerializeField]
    private VectorView vecB;

    [SerializeField]
    private ScalarView scalar;

    // Update is called once per frame
    void Update() {
      float dot = Vector3.Dot(vecA.Direction, vecB.Direction);
      scalar.ScalarValue = dot;
    }
  }
}
