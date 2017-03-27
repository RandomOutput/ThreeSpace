using System;
using UnityEngine;
using Denizen.Utils;
using Denizen.Settings;
using Valve.VR;

namespace Denizen.Input
{
  public class DenizenInputManager : Singleton<DenizenInputManager>
  {
    public delegate bool TryGetTrackedObjectDelegate(out DenizenTrackedObject trackedObject);
    public delegate void StartTrackingObjectDelegate(uint index, DenizenTrackedObject newObject);
    public delegate void WrapperInputEventDelegate(uint index, DenizenTrackedObjectEventData eventData);

    private DenizenTrackedObject[] _trackedObjects;
    private DenizenInputWrapper _inputWrapper;

    public void Initialize(DenizenInputWrapper inputWrapper)
    {
      _inputWrapper = inputWrapper;
      _inputWrapper.StartTrackingObjectCallback = StartTrackingObject;
      _inputWrapper.InputEventCallback = WrapperInputEventCallback;
      _inputWrapper.Connect();
    }

    public DenizenTrackedObject GetTrackedObject(uint id)
    {
      if(id >= _trackedObjects.Length)
      {
        return DenizenTrackedObject.InvalidTrackedObject();
      }

      return _trackedObjects[id];
    }

    public uint TrackedObjectCount()
    {
      return (uint)_trackedObjects.Length;
    }

    public bool TryGetChiralHand(Chirality chirality, out DenizenTrackedObject trackedObject)
    {
      return chirality == Chirality.LEFT ? _inputWrapper.TryGetLeftHand(out trackedObject) : _inputWrapper.TryGetRightHand(out trackedObject);
    }

    protected override void Awake()
    {
      base.Awake();
      PopulateTrackedObjectArray();
    }

    private void PopulateTrackedObjectArray()
    {
      _trackedObjects = new DenizenTrackedObject[DenizenPlatformData.MaxTrackedObjects];
      for (uint i=0; i<_trackedObjects.Length;i++)
      {
        _trackedObjects[i] = DenizenTrackedObject.InvalidTrackedObject();
      }
    }

    void OnEnable()
    {
      if (_inputWrapper != null)
      {
        _inputWrapper.Connect();
      }
    }

    void OnDisable()
    {
      if (_inputWrapper != null)
      {
        _inputWrapper.Disconnect();
      }
    }

    private void WrapperInputEventCallback(uint index, DenizenTrackedObjectEventData eventData)
    {
      if(eventData.EventType == DenizenTrackedObjectEventType.PRIMARY_PRESSED)
      {
        _trackedObjects[index].FirePrimaryPressed();
      }
      else if(eventData.EventType == DenizenTrackedObjectEventType.PRIMARY_RELEASED)
      {
        _trackedObjects[index].FirePrimaryReleased();
      }
    }

    private void StartTrackingObject(uint index, DenizenTrackedObject trackedObject)
    {
      if (index >= _trackedObjects.Length)
      {
        Debug.LogError("index: " + index + ", is out of range. DenizenPlatformData.MaxTrackedObjects = " + DenizenPlatformData.MaxTrackedObjects);
        return;
      }

      _trackedObjects[index] = trackedObject;
    }
  }
}