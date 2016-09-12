using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Arcade.Models;
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
                if (DateTime.Today.DayOfWeek != DayOfWeek.Saturday)
                    return false;

                return DateTime.Now.Hour >= 20;
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
                Settings.Instance.JumboCactpotBoughtTicket = true;
                Logging.Write(Colors.DodgerBlue, $@"[Arcade] It is not time to draw yet, or we don't have a ticket.");
                return false;
            }

            // When you interact for the reward, the first window that pops up is the reward list. You automatically get the MGP reward in the log.
            // Open the window and close the list.
            await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryWeeklyRewardList"));
            var window = RaptureAtkUnitManager.GetWindowByName("LotteryWeeklyRewardList");
            // Close the window
            window.SendAction(1, 3, 1);
            Logging.Write(Colors.DodgerBlue, $@"[Arcade] We have not bought a ticket yet.");
            Settings.Instance.JumboCactpotBoughtTicket = false;
            return true;
        }

        public static async Task<bool> BuyTicket()
        {
            if (Settings.Instance.JumboCactpotBoughtTicket)
            {
                return false;
            }

            await Movement.MoveToLocation(JumboCactpotBrokerLocation, 4);

            var broker = GameObjectManager.GetObjectsByNPCId(JumboCactpotBroker).OrderBy(r => r.Distance2D()).FirstOrDefault();

            if (broker == null)
                return false;

            if (!Core.Player.IsFacing(broker.Location))
                Core.Player.Face(broker);

            broker.Interact();

            await Coroutine.Wait(5000, () => Talk.DialogOpen);

            Talk.Next();

            await Coroutine.Wait(5000, () => SelectString.IsOpen);

            var firstLine = SelectString.Lines().FirstOrDefault();

            if (firstLine != null && firstLine.Contains("payout"))
            {
                SelectString.ClickSlot(4);
                Settings.Instance.JumboCactpotBoughtTicket = true;
                Logging.Write(Colors.DodgerBlue, $@"[Arcade] We already have a Jumbo Cactpot ticket.");
                Settings.Instance.JumboCactpotBuyTime = DateTime.Now;
                return false;
            }

            SelectString.ClickSlot(0);

            await Coroutine.Wait(5000, () => LotteryInputOpen);
            await Coroutine.Sleep(3000);

            var window = RaptureAtkUnitManager.GetWindowByName("LotteryWeeklyInput");
            window.SendAction(1, 3, 3421);

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
            
            return true;
        }
    }
}
