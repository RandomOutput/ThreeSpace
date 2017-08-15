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

using System;
using UnityEngine;
using Denizen.Utils;
using Denizen.Input.Interactions;

namespace ThreeSpace
{
  [RequireComponent(typeof(InteractionNotifier))]
  public class VectorDragger : MonoBehaviour
  {
    private event Action DragStart;
    private event Action DragEnd;

    [SerializeField]
    private Transform _targetTransform;

    private InteractionNotifier _interactionNotifier;

    private Vector3 _startVectorInteractorToTarget;
    private Quaternion _startInteractorOrientation;
    //private Vector3 _targetStartPosition;
    //private Vector3 _interactionStartPosition;

    private bool _isDragging = false;

    void Start()
    {
      _interactionNotifier = GetComponent<InteractionNotifier>();
      _interactionNotifier.InteractionStart += OnInteractionStart;
      _interactionNotifier.InteractionEnd += OnInteractionEnd;
    }

    private void OnInteractionStart()
    {
      _startVectorInteractorToTarget = _targetTransform.position - _interactionNotifier.Interactor.InteractionSource.transform.position;
      _startInteractorOrientation = _interactionNotifier.Interactor.InteractionSource.transform.rotation;
      _isDragging = true;
      DragStart.Fire();
    }

    private void OnInteractionEnd()
    {
      _isDragging = false;
      DragEnd.Fire();
    }

    void Update()
    {
      HandleDrag();
    }

    private void HandleDrag()
    {
      if(!_isDragging)
      {
        return;
      }

      Quaternion rotationDelta = _interactionNotifier.Interactor.InteractionSource.transform.rotation * Quaternion.Inverse(_startInteractorOrientation);
      Vector3 newVectorToTarget = rotationDelta * _startVectorInteractorToTarget;
      Debug.DrawRay(_interactionNotifier.Interactor.InteractionSource.transform.position, newVectorToTarget, Color.magenta);
      Vector3 newTargetPosition = _interactionNotifier.Interactor.InteractionSource.transform.position + newVectorToTarget;
      _targetTransform.position = newTargetPosition;
    }
  }
}
