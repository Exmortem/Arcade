using System.Linq;
using System.Threading.Tasks;
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
            return await OpenGame();
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
            SelectIconString.ClickLineContains("Purchase");
            await Coroutine.Wait(5000, () => Talk.DialogOpen);

            while (Talk.DialogOpen)
            {
                Talk.Next();
                await Coroutine.Yield();
            }

            await Coroutine.Wait(5000, () => SelectYesno.IsOpen);

            while (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                await Coroutine.Yield();
            }

            return await Coroutine.Wait(5000, () => RaptureAtkUnitManager.GetRawControls.Any(r => r.Name == "LotteryDaily"));
        }
    }
}
