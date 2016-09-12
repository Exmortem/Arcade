using System.Threading.Tasks;
using Arcade.Utilities;
using Buddy.Coroutines;
using Clio.Utilities;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Navigation;

namespace Arcade.Tasks
{
    public static class Movement
    {
        public static async Task<bool> MoveToLocation(Vector3 location, float precision)
        {
            while (Core.Player.Location.Distance(location) > precision)
            {
                if (Core.Player.Location.Distance(location) > 50)
                {
                    if (Actionmanager.CanCast(3, Core.Me))
                    {
                        Actionmanager.DoAction(3, Core.Me);
                    }
                }

                Navigator.MoveTo(location);
                Mgp.UpdateTimeSpan();
                await Coroutine.Yield();
            }
            
            Navigator.Clear();
            return true;
        }
    }
}
