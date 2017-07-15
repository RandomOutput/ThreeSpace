using System;
using UnityEngine;

namespace ThreeSpace
{
  public interface IInteractable
  {
    event Action InteractionStart;
    event Action InteractionEnd;

    GameObject ParentObject
    {
      get;
    }

    bool IsInteractedWith
    {
      get;
    }

    IInteractor Interactor
    {
      get;
    }

    bool TryStartInteraction(IInteractor interactor);
    bool TryStopInteraction(IInteractor interactor);
  }
}
