using System;
using System.Linq;
using System.Threading.Tasks;
using Arcade.Models;
using Arcade.Utilities;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

namespace Arcade.Tasks
{
    public static class CrystalTowerStryker
    {
        private const int Id = 2005035;
        private static readonly Vector3 Location = new Vector3() { X = 25.18871f, Y = 4.91873f, Z = 92.24194f };

        private static bool IsOpen
        {
            get
            {
                return RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "Hummer");
            }
        }

        public static async Task<bool> Play()
        {
            await Movement.MoveToLocation(Location, 3);

            await OpenClosestMachine();

            if (!IsOpen)
                return false;

            await Swing();
            return true;
        }

        private static async Task OpenClosestMachine()
        {
            if (IsOpen)
                return;

            var closestMachine = GameObjectManager.GetObjectsByNPCId(Id).OrderBy(r => r.Distance2D()).FirstOrDefault();

            if (closestMachine == null)
                return;

            if (!Core.Player.IsFacing(closestMachine.Location))
                Core.Player.Face(closestMachine);

            closestMachine?.Interact();
            await Coroutine.Wait(5000, () => SelectString.IsOpen);
            SelectString.ClickSlot(0);
            await Coroutine.Wait(2000, () => Core.Memory.Read<byte>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("Hummer").Pointer + 0xD0)) == 4);

            if (!Settings.Instance.FastMode)
            {
                var random = new Random().Next(1500, 3000);
                await Coroutine.Sleep(random);
            }
        }

        private static async Task Swing()
        {
            // Value Pairs: {3, 0xB}, {3, 3}, {3, 0x71B}
            // Pair 1 is unknown
            // Pair 2 is your payout, 3 = Pulverizing (5 MGP), 2 = Crushing (3 MGP), 1 = Glancing (2 MGP), 0 = Weak (0 MGP)
            // Pair 3 is the displayed score on the popup

            var rnd = new Random();
            int payout;

            if (Settings.Instance.RandomizeScores)
            {
                var percentage = rnd.Next(1, 101);

                if (percentage <= 40)
                {
                    payout = percentage <= 20 ? 1 : 2;
                }
                else
                {
                    payout = 3;
                }
            }
            else
            {
                payout = 3;
            }

            var window = RaptureAtkUnitManager.GetWindowByName("Hummer");
            WindowInteraction.SendAction(window, 3, 3, 0xB, 3, (uint)payout, 0, 0);

            //RaptureAtkUnitManager.GetWindowByName("Hummer").SendAction(3, 3, 0xB, 3, payout, 0, 0);
            await Coroutine.Wait(10000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "GoldSaucerReward"));
        }
    }
}
