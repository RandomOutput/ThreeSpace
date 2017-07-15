using System;
using UnityEngine;
using Denizen.Utils;

namespace Denizen.Input
{
  public enum DenizenTrackedObjectClass
  {
    HEADSET,
    INPUT,
    UNKNOWN,
    INVALID
  }

  public enum DenizenTrackedObjectEventType
  {
    PRIMARY_PRESSED,
    PRIMARY_RELEASED,
    TRACKING_STARTED,
    TRACKING_ENDED,
    ID_CHANGED
  }

  public class DenizenTrackedObjectEventData : EventArgs
  {
    public readonly DenizenTrackedObjectEventType EventType;

    public DenizenTrackedObjectEventData(DenizenTrackedObjectEventType eventType)
    {
      EventType = eventType;
    }
  }

  public class DenizenTrackedObject
  {
    public delegate Vector3 PositionDelegate(DenizenTrackedObject trackedObject);
    public delegate Quaternion OrientationDelegate(DenizenTrackedObject trackedObject);
    public delegate Ray InteractionRayDelegate(DenizenTrackedObject trackedObject);
    public delegate void FireEventDelegate();

    public struct Delegates
    {
      public PositionDelegate _positionDelegate;
      public OrientationDelegate _orientationDelegate;
      public InteractionRayDelegate _interactionRayDelegate;
      public FireEventDelegate _registerPrimaryPressedDelegate;
      public FireEventDelegate _registerPrimaryReleasedDelegate;
    }

    public event EventHandler<DenizenTrackedObjectEventData> PrimaryPressed;
    public event EventHandler<DenizenTrackedObjectEventData> PrimaryReleased;
    public event EventHandler<DenizenTrackedObjectEventData> TrackingStarted;
    public event EventHandler<DenizenTrackedObjectEventData> TrackingEnded;
    public event EventHandler<DenizenTrackedObjectEventData> IdChanged;

    public static DenizenTrackedObject InvalidTrackedObject()
    {
      return new DenizenTrackedObject(uint.MaxValue, DenizenTrackedObjectClass.INVALID, new DenizenTrackedObject.Delegates());
    }

    public uint TrackedObjectId
    {
      get { return _trackedObjectId; }
      set
      {
        if (_trackedObjectId == value)
        {
          return;
        }

        _trackedObjectId = value;
        IdChanged.Fire<DenizenTrackedObjectEventData>(this, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.ID_CHANGED));
      }
    }

    public bool IsTracked
    {
      get { return _isTracked; }

      set
      {
        if (_isTracked == value)
        {
          return;
        }

        _isTracked = value;
        if (_isTracked)
        {
          TrackingStarted.Fire<DenizenTrackedObjectEventData>(this, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.TRACKING_STARTED));
        }
        else
        {
          TrackingEnded.Fire<DenizenTrackedObjectEventData>(this, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.TRACKING_ENDED));
        }
      }
    }

    public DenizenTrackedObjectClass TrackedObjectClass
    {
      get { return _trackedObjectClass; }
    }

    private uint _trackedObjectId;
    private bool _isTracked;
    private Delegates _trackedObjectDelegates;
    private DenizenTrackedObjectClass _trackedObjectClass;

    public DenizenTrackedObject(uint trackedObjectId, DenizenTrackedObjectClass trackedObjectClass, Delegates trackedObjectDelegates)
    {
      _trackedObjectId = trackedObjectId;
      _trackedObjectClass = trackedObjectClass;
      _trackedObjectDelegates = trackedObjectDelegates;
    }

    public static implicit operator uint(DenizenTrackedObject trackedObject)
    {
      return trackedObject.TrackedObjectId;
    }

    public bool TryGetInteractionRay(out Ray interactionRay)
    {
      if (_trackedObjectDelegates._interactionRayDelegate == null)
      {
        interactionRay = new Ray();
        return false;
      }

      interactionRay = _trackedObjectDelegates._interactionRayDelegate(this);
      return true;
    }

    public bool TryGetPosition(out Vector3 position)
    {
      if (_trackedObjectDelegates._positionDelegate == null)
      {
        position = Vector3.zero;
        return false;
      }

      position = _trackedObjectDelegates._positionDelegate(this);
      return true;
    }

    public bool TryGetOrientation(out Quaternion orientation)
    {
      if (_trackedObjectDelegates._orientationDelegate == null)
      {
        orientation = Quaternion.identity;
        return false;
      }

      orientation = _trackedObjectDelegates._orientationDelegate(this);
      return true;
    }

    /// <summary>
    /// Internal Use Only
    /// </summary>
    public void FirePrimaryPressed()
    {
      PrimaryPressed.Fire<DenizenTrackedObjectEventData>(this, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.PRIMARY_PRESSED));
    }

    /// <summary>
    /// Internal Use Only
    /// </summary>
    public void FirePrimaryReleased()
    {
      PrimaryReleased.Fire<DenizenTrackedObjectEventData>(this, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.PRIMARY_RELEASED));
    }
  }
}
