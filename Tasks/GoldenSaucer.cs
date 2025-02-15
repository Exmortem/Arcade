﻿using System;
using System.Threading.Tasks;
using System.Windows.Media;
using Arcade.Languages;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;

namespace Arcade.Tasks
{
    public static class GoldenSaucer
    {
        public static bool InZone => WorldManager.ZoneId == 144;

        public static async Task<bool> TeleportToSaucer()
        {
            if (Core.Player.InCombat || Core.Me.IsCasting || MovementManager.IsMoving)
            {
                return false;
            }

            if (!WorldManager.HasAetheryteId(62))
            {
                Logging.Write(Colors.Red, Language.Instance.LogStoppingNoAetheryte);
                TreeRoot.Stop(Language.Instance.LogStoppingNoAetheryte);
                return false;
            }

            await CommonBehaviors.CreateTeleportBehavior(r => 62, r => WorldManager.GetZoneForAetheryteId(144)).ExecuteCoroutine();
            await Coroutine.Wait(TimeSpan.MaxValue, () => !CommonBehaviors.IsLoading || Core.Player.InCombat);
            return true;
        }
    }
}
