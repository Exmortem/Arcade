using Arcade.Models;
using ff14bot;

namespace Arcade.Overlay
{
    public static class ArcadeOverlay
    {
        private static readonly ArcadeOverlayUiComponent ArcadeOverlayComponent = new ArcadeOverlayUiComponent(true);

        public static void Start()
        {
            if (!Core.OverlayManager.IsActive)
            {
                Core.OverlayManager.Activate();
            }

            Core.OverlayManager.AddUIComponent(ArcadeOverlayComponent);
        }

        public static void Stop()
        {
            if (!Core.OverlayManager.IsActive)
                return;
            
            Core.OverlayManager.RemoveUIComponent(ArcadeOverlayComponent);
            Settings.Instance.Save();
        }
    }
}
