using System;

public static class DenizenEventHandlerExtentions {
  public static void Fire<T>(this EventHandler<T> e, object sender, T args) where T : EventArgs
  {
    EventHandler<T> eventSafe = e;
    if(eventSafe == null)
    {
      return;
    }

    eventSafe(sender, args);
  }
}
