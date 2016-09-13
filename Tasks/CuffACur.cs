using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Arcade.Models;
using Arcade.Utilities;
using Arcade.ViewModels;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

namespace Arcade.Tasks
{
    public static class CuffACur
    {
        private const int Id = 2005029;
        private static readonly Vector3 Location = new Vector3() { X = 13.04904f, Y = -5.000005f, Z = -52.66679f };

        private static bool IsOpen
        {
            get
            {
                return RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "PunchingMachine");
            }
        }

        public static async Task<bool> Play()
        {
            await Movement.MoveToLocation(Location, 3);

            await OpenClosestMachine();

            if (!IsOpen)
                return false;

            var randomScore = new Random().Next(1500, 2000);
            await Punch(3, (uint)randomScore);
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
            await Coroutine.Wait(5000, () => SelectString.IsOpen && IsOpen);
            SelectString.ClickSlot(0);
            await Coroutine.Wait(2000, () => Core.Memory.Read<byte>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("PunchingMachine").Pointer + 0xD0)) == 4);

            if (!Settings.Instance.FastMode)
            {
                var random = new Random().Next(1500, 3000);
                await Coroutine.Sleep(random);
            }
        }

        private static async Task Punch(uint payout, uint score)
        {
            // Value Pairs: {3, 0xB}, {3, 3}, {3, 0x71B}
            // Pair 1 is unknown
            // Pair 2 is your payout, 3 = Brutal (5 MGP), 2 = Punishing (3 MGP), 1 = Brusing (2 MGP), 0 = Weak (0 MGP)
            // Pair 3 is the displayed score on the popup

            var window = RaptureAtkUnitManager.GetWindowByName("PunchingMachine");
            WindowInteraction.SendAction(window, 3, 3, 0xB, payout, 3, 3, score);

            //RaptureAtkUnitManager.GetWindowByName("PunchingMachine").SendAction(3, 3, 0xB, payout, 3, 3, score);
            await Coroutine.Wait(10000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "GoldSaucerReward"));
        }
    }
}
