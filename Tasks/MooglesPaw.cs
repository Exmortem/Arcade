using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Arcade.Models;
using Arcade.Utilities;
using Arcade.ViewModels;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

namespace Arcade.Tasks
{
    public static class MooglesPaw
    {
        static MooglesPaw()
        {
            Location = Locations.FirstOrDefault();
            LastLocation = Locations.FirstOrDefault();
        }

        private const int Id = 2005036;

        private static Vector3 Location { get; set; }
        private static Vector3 LastLocation { get; set; }

        private static readonly List<Vector3> Locations = new List<Vector3>()
        {
            new Vector3() { X = 112.2101f, Y = -5.00001f, Z = -57.23045f },
            new Vector3() { X = 122.2405f, Y = -5.000004f, Z = -67.07031f },
        };

        private static bool IsOpen
        {
            get { return RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "UfoCatcher"); }
        }

        public static async Task<bool> Play()
        {
            var count = Locations.Count;
            var randomIndex = new Random().Next(count);

            if (Behaviors.SwitchedGame && !Behaviors.PlayingOnlyOneGame)
            {
                if (Settings.Instance.NeverUseSameMachineTwiceInARow)
                {
                    Location = Locations.FirstOrDefault(r => r != LastLocation);
                }
                else
                {
                    Location = Locations[randomIndex];
                }

                Behaviors.SwitchedGame = false;
                LastLocation = Location;
            }

            await Movement.MoveToLocation(Location, 3);

            await OpenClosestMachine();

            if (!IsOpen)
                return false;

            await Catch();
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

            await Coroutine.Wait(2000, () => Core.Memory.Read<byte>(RaptureAtkUnitManager.GetWindowByName("UfoCatcher").Pointer + 0x189) == 1);     

            if (!Settings.Instance.FastMode)
            {
                var random = new Random().Next(1500, 3000);
                await Coroutine.Sleep(random);
            }
        }

        private static async Task Catch()
        {
            // Value Pairs: {3, 0xB}, {3, 1}, {3, 0x71B}
            // Pair 1 is unknown
            // Pair 2 determines payout, 2 = Small item (5 MGP), 1 = Large item (2 MGP), 0 = Try Again (0 MGP)
            // Pair 3 is unknown

            var rnd = new Random();
            int payout;

            if (Settings.Instance.RandomizeScores)
            {
                var percentage = rnd.Next(1, 101);

                payout = percentage <= 40 ? 1 : 2;
            }
            else
            {
                payout = 2;
            }

            var window = RaptureAtkUnitManager.GetWindowByName("UfoCatcher");
            WindowInteraction.SendAction(window, 3, 3, 0xB, 3, (uint)payout, 3, 0);

            //RaptureAtkUnitManager.GetWindowByName("UfoCatcher").SendAction(3, 3, 0xB, payout, 2, 3, 0);
            await Coroutine.Wait(10000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "GoldSaucerReward"));
        }
    }
}
