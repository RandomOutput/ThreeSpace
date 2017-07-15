using Valve.VR;
using Denizen.Settings;

namespace Denizen
{
  public class DenizenPlatformData
  {
    public static uint MaxTrackedObjects
    {
      get
      {
        PlatformSettings settings = DenizenSettingsInjector.Instance.GetSettings<PlatformSettings>();
        if (settings.Platform == VRPlatform.STEAM_VR)
        {
          return OpenVR.k_unMaxTrackedDeviceCount;
        }
        return 0;
      }
    }
  }
}
