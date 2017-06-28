using System.Threading.Tasks;
using Arcade.Utilities;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Managers;
using ff14bot.Pathing;

namespace Arcade.Tasks
{
    public static class Movement
    {
        public static async Task<bool> MoveToLocation(Vector3 location, float precision)
        {
            if (Core.Player.Location.Distance(location) <= precision)
                return true;

            if (Core.Player.Location.Distance(location) > 50)
            {
                if (ActionManager.CanCast(3, Core.Me))
                {
                    ActionManager.DoAction(3, Core.Me);
                }
            }

            while (await CommonTasks.MoveAndStop(new MoveToParameters(location), 3))
            {
                Mgp.UpdateTimeSpan();
                await Coroutine.Yield();
            }
     
            return true;
        }
    }
}
