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
using LineDrawing;

[ExecuteInEditMode]
public class VectorView : MonoBehaviour {
  [Header("Reference Transforms")]
  [SerializeField]
  private Transform _origin;

  [SerializeField]
  private Transform _endPoint;

  [Header("Visual Parts")]
  [SerializeField]
  private GameObject _arrowHead;

  [SerializeField]
  private GameObject _arrowShaft;

  [SerializeField]
  private GameObject _arrowBase;

  [Header("Visual Constants")]
  [SerializeField]
  private float _shaftScaleOffset;

  [Header("Behavior")]
  [SerializeField]
  private bool _forceNormalized;

  public void SetVector(Vector3 start, Vector3 end)
  {
    Vector3 diff = end - start;
    float dist = diff.magnitude;
    Vector3 dir = diff.normalized;

    SetVector(start, dir, dist);
  }

  public void SetVector(Vector3 origin, Vector3 direction, float magnitude)
  {
    if(_forceNormalized) { magnitude = 1.0f; }

    _arrowShaft.transform.position = origin;

    if (_arrowBase != null)
    {
      _arrowBase.transform.position = origin;
    }

    if (_origin != null)
    {
      _origin.position = origin;
    }

    if(_endPoint != null)
    {
      _endPoint.transform.position = origin + direction * magnitude;
    }

    _arrowHead.transform.forward = direction.normalized;
    _arrowShaft.transform.forward = direction.normalized;
    _arrowHead.transform.position = _endPoint.position;
    _arrowShaft.transform.localScale = new Vector3(1, 1, Mathf.Max(0, magnitude - _shaftScaleOffset));
  }

  public Vector3 Origin
  {
    get {
      if(_origin == null)
      {
        return Vector3.zero;
      }

      return _origin.transform.position;
    }
    set {
      if(_origin == null)
      {
        return;
      }

      SetVector(value, _endPoint.transform.position);
    }
  }

  public Vector3 EndPoint
  {
    get {
      if(_endPoint == null)
      {
        return Vector3.zero;
      }

      return _endPoint.transform.position;
    }
    set {
      if(_endPoint == null)
      {
        return;
      }

      SetVector(_origin.transform.position, value);
    }
  }

  public Vector3 Direction
  {
    get { return (_endPoint.transform.position - _origin.transform.position).normalized; }
  }

  public float Magnitude
  {
    get { return (_endPoint.transform.position - _origin.transform.position).magnitude; }
  }

  public Vector3 Vector
  {
    get { return Direction * Magnitude; }
  }

  protected void Update()
  {
    SetVector(_origin.position, _endPoint.position);
  }
}
