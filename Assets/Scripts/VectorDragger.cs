using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Denizen.Utils;

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
