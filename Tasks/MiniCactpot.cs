using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arcade.Languages;
using Arcade.Models;
using Arcade.Utilities;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.RemoteWindows;

namespace Arcade.Tasks
{
    public static class MiniCactpot
    {
        private const int MiniCactpotBroker = 1010445;
        private static readonly Vector3 MiniCactpotBrokerLocation = new Vector3() { X = -46.35843f, Y = 1.6f, Z = 20.86036f };

        public static async Task<bool> Play()
        {
            if (!Settings.Instance.MiniCactpot)
                return false;

            if (Settings.Instance.MiniCactpotPlayedToday)
                return false;

            if (!await OpenGame())
                return false;

            while (await Solve())
            {
                await Coroutine.Yield();
            }

            await PickARow();

            var window = RaptureAtkUnitManager.GetWindowByName("LotteryDaily");

            while (RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryDaily"))
            {
                WindowInteraction.SendAction(window, 1, 3, 0xFFFFFFFF);
                await Coroutine.Yield();
            }

            await Coroutine.Sleep(1000);

            return true;
        }

        private static async Task<bool> OpenGame()
        {
            if (RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryDaily"))
                return true;

            await Movement.MoveToLocation(MiniCactpotBrokerLocation, 1);

            var broker = GameObjectManager.GetObjectsByNPCId(MiniCactpotBroker).OrderBy(r => r.Distance2D()).FirstOrDefault();

            if (broker == null)
                return false;

            if (!Core.Player.IsFacing(broker.Location))
                Core.Player.Face(broker);

            broker.Interact();

            await Coroutine.Wait(5000, () => SelectIconString.IsOpen);
            
            SelectIconString.ClickLineContains(Language.Instance.MiniCactpotPurchaseTicket);

            await Coroutine.Wait(5000, () => Talk.DialogOpen);

            while (Talk.DialogOpen)
            {
                Talk.Next();
                await Coroutine.Yield();
            }

            if (!await Coroutine.Wait(5000, () => SelectYesno.IsOpen))
            {
                Settings.Instance.MiniCactpotPlayedToday = true;
                return false;
            }

            while (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                await Coroutine.Yield();
            }

            if (!await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryDaily")))
                return false;

            await Coroutine.Sleep(3000);
            return true;
        }

        private static async Task<bool> Solve()
        {
            if (RaptureAtkUnitManager.GetRawControls.All(r => r.Name != "LotteryDaily"))
                return false;
                       
            // If there are 4 turned values on the cactpot ticket return
            if (CactpotIndexValues.Values.Count(r => r > 0) == 4)
                return false;

            // Turn the corners first
            if (CactpotIndexValues[0] == 0)
            {
                return await TurnASlot(0);
            }

            if (CactpotIndexValues[2] == 0)
            {
                return await TurnASlot(2);
            }

            if (CactpotIndexValues[6] == 0)
            {
                return await TurnASlot(6);
            }

            if (CactpotIndexValues[8] == 0)
            {
                return await TurnASlot(8);
            }

            // No corners, turn the first available slot
            var index = CactpotIndexValues.FirstOrDefault(r => r.Value == 0);
            return await TurnASlot((uint)index.Key);
        }

        private static async Task<bool> PickARow()
        {
            if (RaptureAtkUnitManager.GetRawControls.All(r => r.Name != "LotteryDaily"))
                return false;

            var window = RaptureAtkUnitManager.GetWindowByName("LotteryDaily");

            // Start looking for a row that has a value of 6 and work our way down to 1
            for (var i = 6; i > 0; i--)
            {
                var priorityRow = CactpotRowValues.FirstOrDefault(r => r.Value == i).Value;

                if (priorityRow == 0)
                {
                    continue;
                }
              
                WindowInteraction.SendAction(window, 2, 3, 2, 3, (uint)priorityRow);
                await Coroutine.Sleep(3000);
            }
            
            var index = CactpotRowValues.OrderBy(r => r.Value).FirstOrDefault(r => r.Value != 0);
            WindowInteraction.SendAction(window, 2, 3, 2, 3, (uint)index.Key);
            await Coroutine.Sleep(3000);
            return true;
        }

        private static async Task<bool> TurnASlot(uint index)
        {
            var window = RaptureAtkUnitManager.GetWindowByName("LotteryDaily");
            WindowInteraction.SendAction(window, 2, 3, 1, 3, index);
            await Coroutine.Sleep(1500);
            return true;
        }

        private static Dictionary<int, int> CactpotPayouts = new Dictionary<int, int>()
        {
            {6, 10000},
            {7, 36},
            {8, 720},
            {9, 360},
            {10, 80},
            {11, 252},
            {12, 108},
            {13, 72},
            {14, 54},
            {15, 180},
            {16, 72},
            {17, 180},
            {18, 119},
            {19, 36},
            {20, 306},
            {21, 1080},
            {22, 144},
            {23, 1800},
            {24, 3600},
        };


        private static Dictionary<int, int> CactpotIndexValues
        {
            get
            {
                var dictionary = new Dictionary<int, int>
                {
                    {0, ValueOfIndex0},
                    {1, ValueOfIndex1},
                    {2, ValueOfIndex2},
                    {3, ValueOfIndex3},
                    {4, ValueOfIndex4},
                    {5, ValueOfIndex5},
                    {6, ValueOfIndex6},
                    {7, ValueOfIndex7},
                    {8, ValueOfIndex8}
                };

                return dictionary;
            }
        }

        private static Dictionary<int, int> CactpotRowValues
        {
            get
            {
                var dictionary = new Dictionary<int, int>
                {
                    {0, ValueOfColumn0},
                    {1, ValueOfColumn1},
                    {2, ValueOfColumn2},
                    {3, ValueOfColumn3},
                    {4, ValueOfColumn4},
                    {5, ValueOfColumn5},
                    {6, ValueOfColumn6},
                    {7, ValueOfColumn7}
                };

                return dictionary;
            }
        }

        private static int ValueOfColumn0 => ValueOfIndex0 + ValueOfIndex4 + ValueOfIndex8;
        private static int ValueOfColumn1 => ValueOfIndex0 + ValueOfIndex3 + ValueOfIndex6;
        private static int ValueOfColumn2 => ValueOfIndex1 + ValueOfIndex4 + ValueOfIndex7;
        private static int ValueOfColumn3 => ValueOfIndex2 + ValueOfIndex5 + ValueOfIndex8;
        private static int ValueOfColumn4 => ValueOfIndex2 + ValueOfIndex4 + ValueOfIndex6;
        private static int ValueOfColumn5 => ValueOfIndex0 + ValueOfIndex1 + ValueOfIndex2;
        private static int ValueOfColumn6 => ValueOfIndex3 + ValueOfIndex4 + ValueOfIndex5;
        private static int ValueOfColumn7 => ValueOfIndex6 + ValueOfIndex7 + ValueOfIndex8;
        
        private static int ValueOfIndex0 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x23C);
        private static int ValueOfIndex1 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x240);
        private static int ValueOfIndex2 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x244);
        private static int ValueOfIndex3 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x248);
        private static int ValueOfIndex4 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x24C);
        private static int ValueOfIndex5 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x250);
        private static int ValueOfIndex6 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x254);
        private static int ValueOfIndex7 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x258);
        private static int ValueOfIndex8 => Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x25C);
    }
}
