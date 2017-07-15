using Denizen.Settings;
using Denizen.Utils;
using Denizen.Input.Wrappers;

namespace Denizen.Input
{
  public class DenizenInputManagerInjector : Singleton<DenizenInputManagerInjector>
  {
    protected void Start()
    {
      PlatformSettings settings = DenizenSettingsInjector.Instance.GetSettings<PlatformSettings>();
      DenizenInputWrapper wrapper = null;
      DenizenInputManager inputManager = null;

      if (settings.Platform == VRPlatform.STEAM_VR)
      {
        wrapper = gameObject.AddComponent<DenizenSteamVRInputWrapper>();
        inputManager = gameObject.AddComponent<DenizenInputManager>();
      }

      if(inputManager == null || wrapper == null)
      {
        return;
      }

      inputManager.Initialize(wrapper);
      Destroy(this);
    }
  }
}
