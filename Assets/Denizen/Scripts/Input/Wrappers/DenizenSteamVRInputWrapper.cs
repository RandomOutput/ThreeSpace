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
using Denizen.Utils;
using UnityEngine;
using Valve.VR;

namespace Denizen.Input.Wrappers
{
  public class DenizenSteamVRInputWrapper : DenizenInputWrapper
  {
    private CVRSystem _system;
    private static Vector3[] _positions;
    private static Quaternion[] _rotations;

    public override void Connect()
    {
      SteamVR_Events.DeviceConnected.Listen(OnDeviceConnectionChanged);
      SteamVR_Events.NewPoses.Listen(OnNewPoses);
      SteamVR_Events.System(EVREventType.VREvent_ButtonPress).Listen(OnButtonPress);
      SteamVR_Events.System(EVREventType.VREvent_ButtonUnpress).Listen(OnButtonUnpress);
    }

    public override void Disconnect()
    {
      SteamVR_Events.DeviceConnected.Remove(OnDeviceConnectionChanged);
      SteamVR_Events.NewPoses.Remove(OnNewPoses);
      SteamVR_Events.System(EVREventType.VREvent_ButtonPress).Remove(OnButtonPress);
      SteamVR_Events.System(EVREventType.VREvent_ButtonUnpress).Remove(OnButtonUnpress);
    }

    public override bool SupportsHandAccessors()
    {
      return true;
    }

    public override bool TryGetLeftHand(out DenizenTrackedObject trackedObject)
    {
      return TryGetChiralHand(Chirality.LEFT, out trackedObject);
    }

    public override bool TryGetRightHand(out DenizenTrackedObject trackedObject)
    {
      return TryGetChiralHand(Chirality.RIGHT, out trackedObject);
    }

    protected void Awake()
    {
      _system = OpenVR.System;
      PopulateSteamVRTrackingArrays();
    }

    private void OnButtonPress(VREvent_t e)
    {
      uint button = e.data.controller.button;
      if (button == (uint)EVRButtonId.k_EButton_SteamVR_Trigger)
      {
        if (InputEventCallback != null)
        {
          InputEventCallback(e.trackedDeviceIndex, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.PRIMARY_PRESSED));
        };
      }
    }

    private void OnButtonUnpress(VREvent_t e)
    {
      uint button = e.data.controller.button;
      if (button == (uint)EVRButtonId.k_EButton_SteamVR_Trigger)
      {
        if (InputEventCallback != null)
        {
          InputEventCallback(e.trackedDeviceIndex, new DenizenTrackedObjectEventData(DenizenTrackedObjectEventType.PRIMARY_RELEASED));
        };
      }
    }

    private void PopulateSteamVRTrackingArrays()
    {
      _positions = new Vector3[DenizenPlatformData.MaxTrackedObjects];
      _rotations = new Quaternion[DenizenPlatformData.MaxTrackedObjects];

      for (uint i = 0; i < DenizenPlatformData.MaxTrackedObjects; i++)
      {
        _positions[i] = Vector3.zero;
        _rotations[i] = Quaternion.identity;
      }
    }

    private void OnDeviceConnectionChanged(int index, bool connected)
    {
      DenizenTrackedObject trackedObject = DenizenInputManager.Instance.GetTrackedObject((uint)index);
      trackedObject.IsTracked = false; // Invalidate the existing tracked object as we have a new one.

      if (connected)
      {
        if (_system != null)
        {
          ETrackedDeviceClass deviceClass = _system.GetTrackedDeviceClass((uint)index);

          if (deviceClass == ETrackedDeviceClass.Controller)
          {
            trackedObject = CreateSteamController((uint)index);
          }
          else if (deviceClass == ETrackedDeviceClass.HMD)
          {
            trackedObject = CreateHeadset((uint)index);
          }
          else
          {
            trackedObject = DenizenTrackedObject.InvalidTrackedObject();
          }

          trackedObject.IsTracked = true;

          StartTrackingObjectCallback((uint)index, trackedObject);
        }
      }
    }

    private void OnNewPoses(TrackedDevicePose_t[] poses)
    {
      SteamVR_Utils.RigidTransform transformUtil;
      for (int i = 0; i < DenizenPlatformData.MaxTrackedObjects; i++)
      {
        if (poses[i].bPoseIsValid)
        {
          transformUtil = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

          _rotations[i] = transformUtil.rot;
          _positions[i] = transformUtil.pos;
        }
      }
    }

    private DenizenTrackedObject CreateSteamController(uint trackedObjectId)
    {
      var trackedObjectDelegates = new DenizenTrackedObject.Delegates();
      trackedObjectDelegates._positionDelegate = GetPosition;
      trackedObjectDelegates._orientationDelegate = GetOrientation;
      trackedObjectDelegates._interactionRayDelegate = GetInteractionRay;
      return new DenizenTrackedObject(trackedObjectId, DenizenTrackedObjectClass.INPUT, trackedObjectDelegates);
    }

    private DenizenTrackedObject CreateHeadset(uint trackedObjectId)
    {
      var trackedObjectDelegates = new DenizenTrackedObject.Delegates();
      return new DenizenTrackedObject(trackedObjectId, DenizenTrackedObjectClass.INPUT, trackedObjectDelegates);
    }

    private bool TryGetChiralHand(Chirality chirality, out DenizenTrackedObject trackedObject)
    {
      uint index = _system.GetTrackedDeviceIndexForControllerRole(chirality == Chirality.LEFT ? ETrackedControllerRole.LeftHand : ETrackedControllerRole.RightHand);
      trackedObject = DenizenInputManager.Instance.GetTrackedObject(index);
      return trackedObject.IsTracked;
    }

    private Vector3 GetPosition(DenizenTrackedObject trackedObject)
    {
      return _positions[trackedObject.TrackedObjectId];
    }

    private Quaternion GetOrientation(DenizenTrackedObject trackedObject)
    {
      return _rotations[trackedObject.TrackedObjectId];
    }

    private Ray GetInteractionRay(DenizenTrackedObject trackedObject)
    {
      return new Ray(DenizenCameraManager.Instance.transform.TransformPoint(GetPosition(trackedObject)), DenizenCameraManager.Instance.transform.TransformDirection(GetOrientation(trackedObject) * Vector3.forward));
    }
  }
}
