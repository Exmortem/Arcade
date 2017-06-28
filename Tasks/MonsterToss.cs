using System;
using System.Collections.Generic;
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
    public static  class MonsterToss
    {
        static MonsterToss()
        {
            Location = Locations.FirstOrDefault();
            LastLocation = Locations.FirstOrDefault();
        }

        private const int Id = 2004804;

        private static Vector3 Location { get; set; }
        private static Vector3 LastLocation { get; set; }

        private static readonly List<Vector3> Locations = new List<Vector3>()
        {
            new Vector3() { X = 35.42477f, Y = 5.193397f, Z = 17.80501f },
            new Vector3() { X = 43.49005f, Y = 5.194275f, Z = 17.04697f },
        };

        private static bool IsOpen
        {
            get
            {
                return RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "BasketBall");
            }
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

            await Throw();
            return true;
        }

        private static async Task KeepThrowing()
        {
            if (!IsOpen)
                return;

            await Throw();
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

            await Coroutine.Wait(2000, () => Core.Memory.Read<byte>(RaptureAtkUnitManager.GetWindowByName("BasketBall").Pointer + 0x189) == 1);
            
            if (!Settings.Instance.FastMode)
            {
                var random = new Random().Next(1500, 3000);
                await Coroutine.Sleep(random);
                Mgp.UpdateTimeSpan();
            }
        }

        private static async Task Throw()
        {
            // Value Pairs: {3, 0xB}, {3, 1}, {3, 0x71B}
            // Pair 1 is unknown
            // Pair 2 is shot status, 1 = Nice Shot, 0 = Miss
            // Pair 3 is unknown

            var shot = 1;

            if (Settings.Instance.RandomizeScores)
            {
                shot = new Random().Next(1,101) > 30 ? 1 : 0;
            }

            var window = RaptureAtkUnitManager.GetWindowByName("BasketBall");
            WindowInteraction.SendAction(window, 3, 3, 0xB, 3, (uint)shot, 3, 0);

            await Coroutine.Sleep(5000);
            Mgp.UpdateTimeSpan();

            while (IsOpen)
            {
                await KeepThrowing();
                Mgp.UpdateTimeSpan();
                await Coroutine.Yield();
            }

            await Coroutine.Wait(10000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "GoldSaucerReward"));
        }
    }
}
