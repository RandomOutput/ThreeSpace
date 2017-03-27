using Denizen.Settings;
using Denizen.Utils;
using Denizen.Input.Wrappers;

namespace Denizen.Input
{
  public class DenizenInputManagerInjector : Singleton<DenizenInputManagerInjector>
  {
    protected override void Awake()
    {
      base.Awake();
      if (PlatformSettings.Instance.Platform == VRPlatform.STEAM_VR)
      {
        DenizenSteamVRInputWrapper steamVRWrapper = gameObject.AddComponent<DenizenSteamVRInputWrapper>();
        DenizenInputManager inputManager = gameObject.AddComponent<DenizenInputManager>();
        inputManager.Initialize(steamVRWrapper);
        Destroy(this);
      }
    }
  }
}
