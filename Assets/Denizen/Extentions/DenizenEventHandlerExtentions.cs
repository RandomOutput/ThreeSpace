using System;

namespace Denizen.Utils
{
  public static class DenizenEventHandlerExtentions
  {
    public static void Fire<T>(this EventHandler<T> e, object sender, T args) where T : EventArgs
    {
      EventHandler<T> eventSafe = e;
      if (eventSafe == null)
      {
        return;
      }

      eventSafe(sender, args);
    }

    public static void Fire(this Action e)
    {
      Action actionSafe = e;
      if(actionSafe == null)
      {
        return;
      }

      actionSafe();
    }
  }
}
