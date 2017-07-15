using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeSpace
{
  [RequireComponent(typeof(BoolTrigger))]
  [RequireComponent(typeof(InteractionNotifier))]
  public class InteractionTrigger : MonoBehaviour
  {

    private InteractionNotifier _interactionNotifier;
    private BoolTrigger _boolTrigger;

    private void Awake()
    {
      _interactionNotifier = GetComponent<InteractionNotifier>();
      _boolTrigger = GetComponent<BoolTrigger>();
    }

    // Use this for initialization
    void Start()
    {
      _interactionNotifier.InteractionEnd += _interactionNotifier_InteractionEnd;
    }

    private void _interactionNotifier_InteractionEnd()
    {
      _boolTrigger.Trigger();
    }
  }
}