using System;
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
    public static class JumboCactpot
    {
        private const int JumboCactpotBroker = 1010446;
        private static readonly Vector3 JumboCactpotBrokerLocation = new Vector3() { X = 122.8503f, Y = 13.00134f, Z = -9.384338f };

        private const int CactpotCashier = 1010451;
        private static readonly Vector3 CactpotCashierLocation = new Vector3() { X = 126.4209f, Y = 13.00497f, Z = -20.24878f };

        private static bool TimeToCollect
        {
            get
            {
                if (!Settings.Instance.JumboCactpotBoughtTicket)
                    return false;

                // If the datetime year is 1 it means the setting is defaulted and the ticket hasn't been bought yet or the settings file deleted
                if (Settings.Instance.JumboCactpotBuyTime.Year == 1)
                {
                    Settings.Instance.JumboCactpotBoughtTicket = false;
                    return true;
                }

                var dayBoughtTicket = Settings.Instance.JumboCactpotBuyTime;
                var daysTillCashIn = ((int) DayOfWeek.Saturday - (int) dayBoughtTicket.DayOfWeek + 7) % 7;

                if (dayBoughtTicket.AddDays(daysTillCashIn) < DateTime.Now)
                {
                    //Logging.Write(Colors.Red, $@"{dayBoughtTicket.AddDays(dayWeCanCashIn)}");

                    var now = DateTime.Now;

                    if (now.DayOfWeek == DayOfWeek.Saturday)
                    {
                        if (now.Hour < 20)
                            return false;
                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        private static bool LotteryInputOpen
        {
            get
            {
                return RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryWeeklyInput");
            }
        }

        public static async Task<bool> CollectReward()
        {
            if (!Settings.Instance.JumboCactpot)
                return false;

            if (!TimeToCollect)
                return false;

            await Movement.MoveToLocation(CactpotCashierLocation, 4);

            var broker = GameObjectManager.GetObjectsByNPCId(CactpotCashier).OrderBy(r => r.Distance2D()).FirstOrDefault();

            if (broker == null)
                return false;

            if (!Core.Player.IsFacing(broker.Location))
                Core.Player.Face(broker);

            broker.Interact();

            await Coroutine.Sleep(1000);

            if (Talk.DialogOpen)
            {
                while (Talk.DialogOpen)
                {
                    Talk.Next();
                    await Coroutine.Sleep(1000);
                    await Coroutine.Yield();
                }

                // This probably means we havent bought a ticket, or it's not time to draw yet
                await Coroutine.Wait(5000, () => !Talk.DialogOpen);
                //Settings.Instance.JumboCactpotBoughtTicket = true;
                Logging.Write(Colors.DodgerBlue, Language.Instance.CactpotNotTimeToDrawYet);
                return await BuyTicket();
            }

            // When you interact for the reward, the first window that pops up is the reward list. You automatically get the MGP reward in the log.
            // Open the window and close the list.
            await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryWeeklyRewardList"));
            var window = RaptureAtkUnitManager.GetWindowByName("LotteryWeeklyRewardList");

            while (RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryWeeklyRewardList"))
            {
                WindowInteraction.SendAction(window, 1, 3, 0);
                await Coroutine.Yield();
            }

            Settings.Instance.JumboCactpotBoughtTicket = false;
            return true;
        }

        public static async Task<bool> BuyTicket()
        {
            if (Settings.Instance.JumboCactpotBoughtTicket)
            {
                return false;
            }

            await Movement.MoveToLocation(JumboCactpotBrokerLocation, 3);

            var broker = GameObjectManager.GetObjectsByNPCId(JumboCactpotBroker).OrderBy(r => r.Distance2D()).FirstOrDefault();

            if (broker == null)
                return false;

            if (!Core.Player.IsFacing(broker.Location))
                Core.Player.Face(broker);

            broker.Interact();

            await Coroutine.Wait(5000, () => Talk.DialogOpen);

            while (Talk.DialogOpen)
            {
                Talk.Next();
                await Coroutine.Yield();
            }

            if (!await Coroutine.Wait(2500, () => SelectString.IsOpen))
            {
                Settings.Instance.JumboCactpotBoughtTicket = true;
                Settings.Instance.JumboCactpotBuyTime = DateTime.Now;
                return true;
            }

            var firstLine = SelectString.Lines().FirstOrDefault();

            if (firstLine != null && firstLine.Contains(Language.Instance.CactpotPayout))
            {
                SelectString.ClickSlot(4);
                Settings.Instance.JumboCactpotBoughtTicket = true;
                Logging.Write(Colors.DodgerBlue, Language.Instance.CactpotAlreadyHaveTicket);
                Settings.Instance.JumboCactpotBuyTime = DateTime.Now;
                return false;
            }

            SelectString.ClickSlot(0);

            await Coroutine.Wait(5000, () => LotteryInputOpen);
            await Coroutine.Sleep(3000);

            var lotteryNumber = new Random().Next(0000, 9999);

            var window = RaptureAtkUnitManager.GetWindowByName("LotteryWeeklyInput");

            WindowInteraction.SendAction(window, 1, 3, (uint)lotteryNumber);
            //window.SendAction(1, 3, (uint)lotteryNumber);

            await Coroutine.Wait(5000, () => SelectYesno.IsOpen);
            SelectYesno.ClickYes();
            Settings.Instance.JumboCactpotBoughtTicket = true;

            await Coroutine.Sleep(1000);

            while (Talk.DialogOpen)
            {
                Talk.Next();
                await Coroutine.Sleep(1000);
                await Coroutine.Yield();
            }

            Settings.Instance.JumboCactpotBuyTime = DateTime.Now;
            Settings.Instance.JumboCactpotBoughtTicket = true;
            return true;
        }
    }
}
