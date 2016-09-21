using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Arcade.Languages;
using Arcade.Models;
using Arcade.Utilities;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Helpers;
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

            if (!await Coroutine.Wait(2500, () => SelectYesno.IsOpen))
            {
                Settings.Instance.MiniCactpotPlayedToday = true;
                return false;
            }

            while (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                await Coroutine.Yield();
            }

            if (!await Coroutine.Wait(2500, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryDaily")))
                return false;

            await Coroutine.Sleep(2500);
            return true;
        }

        private static async Task<bool> Solve()
        {
            if (RaptureAtkUnitManager.GetRawControls.All(r => r.Name != "LotteryDaily"))
                return false;
                       
            // If there are 4 turned values on the cactpot ticket return
            if (CactpotIndexValues.Values.Count(r => r > 0) == 4)
                return false;

            Possibilities.Clear();
            Values.Clear();

            foreach (var value in CactpotIndexValues)
            {
                if (value.Value != 0)
                    Values.Add(value.Value);
            }

            Values.Sort();

            for (var a = 1; a < 10; a++)
            {
                if (!Values.Contains(a))
                    Possibilities.Add(a);
            }

            // Pick a slot from the best row
            var bestRow = GetBestRowValue();
            Logging.Write(Colors.Red, $@"[Arcade] Best row value: {bestRow}");

            foreach (var scratchOff in CactpotRows[bestRow])
            {
                if (GetValueOfIndex(scratchOff) > 0)
                    continue;

                Logging.Write(Colors.Red, $@"[Arcade] Picking Slot: {scratchOff}");
                return await TurnASlot((uint) scratchOff);
            }

            var index = CactpotIndexValues.FirstOrDefault(r => r.Value == 0);
            return await TurnASlot((uint)index.Key);
        }

        private static async Task<bool> PickARow()
        {
            if (RaptureAtkUnitManager.GetRawControls.All(r => r.Name != "LotteryDaily"))
                return false;

            var window = RaptureAtkUnitManager.GetWindowByName("LotteryDaily");

            Possibilities.Clear();
            Values.Clear();

            foreach (var value in CactpotIndexValues)
            {
                if (value.Value != 0)
                    Values.Add(value.Value);
            }

            for (var a = 1; a < 10; a++)
            {
                if (!Values.Contains(a))
                    Possibilities.Add(a);
            }

            var bestRow = GetBestRowValue();
            WindowInteraction.SendAction(window, 2, 3, 2, 3, (uint)bestRow);
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

        private static Dictionary<int, List<int>> CactpotRows
        {
            get
            {
                var dictionary = new Dictionary<int, List<int>>()
                {
                    {0, new List<int>() { 0, 4, 8 } },
                    {1, new List<int>() { 0, 3, 6 } },
                    {2, new List<int>() { 1, 4, 7 } },
                    {3, new List<int>() { 2, 5, 8 } },
                    {4, new List<int>() { 2, 4, 6 } },
                    {5, new List<int>() { 0, 1, 2 } },
                    {6, new List<int>() { 3, 4, 5 } },
                    {7, new List<int>() { 6, 7, 8 } },
                };

                return dictionary;
            }
        }


        private static int GetValueOfIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return ValueOfIndex0;
                case 1:
                    return ValueOfIndex1;
                case 2:
                    return ValueOfIndex2;
                case 3:
                    return ValueOfIndex3;
                case 4:
                    return ValueOfIndex4;
                case 5:
                    return ValueOfIndex5;
                case 6:
                    return ValueOfIndex6;
                case 7:
                    return ValueOfIndex7;
                case 8:
                    return ValueOfIndex8;
                default:
                    return 0;
            }
        }

        private static int ValueOfIndex0 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3B8) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x23C);


        private static int ValueOfIndex1 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3BC) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x240);


        private static int ValueOfIndex2 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3C0) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x244);

        private static int ValueOfIndex3 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3C4) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x248);

        private static int ValueOfIndex4 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3C8) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x24C);

        private static int ValueOfIndex5 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3CC) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x250);

        private static int ValueOfIndex6 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3D0) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x254);

        private static int ValueOfIndex7 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3D4) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x258);

        private static int ValueOfIndex8 => Environment.Is64BitProcess ? Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x70) + 0xB0) + 0x3D8) :
                                                                         Core.Memory.Read<int>(Core.Memory.Read<IntPtr>(Core.Memory.Read<IntPtr>(RaptureAtkUnitManager.GetWindowByName("LotteryDaily").Pointer + 0x4C) + 0x10) + 0x25C);


        private static readonly List<int> Values = new List<int>();
        private static readonly List<int> Possibilities = new List<int>();

        private static int GetBestRowValue()
        {
            var valuesDict = new Dictionary<int, int>();

            foreach (var row in CactpotRows)
            {
                var value = GetRowValue(GetValueOfIndex(row.Value[0]), GetValueOfIndex(row.Value[1]), GetValueOfIndex(row.Value[2]));
                valuesDict.Add(row.Key, value);
            }

            foreach (var row in valuesDict)
            {
                Logging.Write(Colors.BlueViolet, $@"[Arcade] {row.Key} : {row.Value}");
            }

            var bestRow = valuesDict.OrderByDescending(r => r.Value).First();
            Logging.Write(Colors.BlueViolet, $@"[Arcade] Best row value: {bestRow.Value}");
            return bestRow.Key;
        }

        private static int GetRowValue(int a, int b, int c)
        {

            if (a == 0 && b == 0 && c == 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var poss1 = Possibilities;

                for (var cap = poss1.Count - 1; cap >= 0; cap--)
                {
                    var index = poss1[cap]; 
                    var poss2 = poss1.ToList();
                    poss2.RemoveAt(cap); 
                    for (var cap2 = poss2.Count - 1; cap2 >= 0; cap2--)
                    {
                        var index2 = index + poss2[cap2];
                        var poss3 = poss2;
                        poss3.RemoveAt(cap2);
                        for (var cap3 = poss3.Count - 1; cap3 >= 0; cap3--)
                        {
                            payout += PayoutCalc(index2 + poss3[cap3]);
                            payoutNr += 1;
                        }
                    }
                }
                return payout / payoutNr;
            }

            if (a == 0 && b == 0 && c != 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var poss1 = Possibilities.ToList();
                for (var cap = poss1.Count - 1; cap >= 0; cap--)
                {
                    var index = poss1[cap]; 
                    var poss2 = poss1.ToList();
                    poss2.RemoveAt(cap); 
                    for (var cap2 = poss2.Count - 1; cap2 >= 0; cap2--)
                    {
                        payout += PayoutCalc(index + c + poss2[cap2]);
                        payoutNr += 1;
                    }
                }
                return payout / payoutNr;
            }

            if (a == 0 && b != 0 && c == 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var poss1 = Possibilities.ToList();
                for (var cap = poss1.Count - 1; cap >= 0; cap--)
                {
                    var index = poss1[cap];
                    var poss2 = poss1.ToList();
                    poss2.RemoveAt(cap);
                    for (var cap2 = poss2.Count - 1; cap2 >= 0; cap2--)
                    {
                        payout += PayoutCalc(index + b + poss2[cap2]);
                        payoutNr += 1;
                    }
                }
                return payout / payoutNr;
            }

            if (a == 0 && b != 0 && c != 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var index = 0;
                var poss1 = Possibilities.ToList();
                foreach (var t in poss1)
                {
                    index = index + b + c + t;
                    payout += PayoutCalc(index);
                    index = 0;
                    payoutNr += 1;
                }
                return payout / payoutNr;
            }

            if (a != 0 && b == 0 && c == 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var poss1 = Possibilities.ToList();

                for (var cap = poss1.Count - 1; cap >= 0; cap--)
                {
                    var index = poss1[cap];
                    var poss2 = poss1.ToList();
                    poss2.RemoveAt(cap); 
                    for (var cap2 = poss2.Count - 1; cap2 >= 0; cap2--)
                    {
                        payout += PayoutCalc(index + a + poss2[cap2]);
                        payoutNr += 1;
                    }
                }
                return payout / payoutNr;
            }
 
            if (a != 0 && b == 0 && c != 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var index = 0;
                var poss1 = Possibilities.ToList();
                foreach (var t in poss1)
                {
                    index = index + a + c + t;
                    payout += PayoutCalc(index);
                    index = 0;
                    payoutNr += 1;
                }
                return payout / payoutNr;
            }

            if (a != 0 && b != 0 && c == 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var index = 0;
                var poss1 = Possibilities.ToList();
                foreach (var t in poss1)
                {
                    index = index + a + b + t;
                    payout += PayoutCalc(index);
                    index = 0;
                    payoutNr += 1;
                }
                return payout / payoutNr;
            }

            // ReSharper disable once InvertIf
            if (a != 0 && b != 0 && c != 0)
            {
                var payout = 0;
                var payoutNr = 0;
                var index = 0;
                index = index + a + b + c;
                payoutNr = payoutNr + 1;
                payout = payout + PayoutCalc(index);
                return payout / payoutNr;
            }

            return 0;
        }

        private static int PayoutCalc(int index)
        {
            switch (index)
            {
                case 6:
                    return 10000;
                case 7:
                    return 36;
                case 8:
                    return 720;
                case 9:
                    return 360;
                case 10:
                    return 80;
                case 11:
                    return 252;
                case 12:
                    return 108;
                case 13:
                    return 72;
                case 14:
                    return 54;
                case 15:
                    return 180;
                case 16:
                    return 72;
                case 17:
                    return 180;
                case 18:
                    return 119;
                case 19:
                    return 36;
                case 20:
                    return 306;
                case 21:
                    return 1080;
                case 22:
                    return 144;
                case 23:
                    return 1800;
                case 24:
                    return 3600;
                default:
                    return 0;
            }
        }
    }
}
