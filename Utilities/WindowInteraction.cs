using System;
using ff14bot;
using ff14bot.Managers;

namespace Arcade.Utilities
{
    public static class WindowInteraction
    {
        public static void SendAction(AtkAddonControl control, int count, params uint[] pairs)
        {
            var method = control.GetType().GetMethod("SendAction");
            var parameters = new object[2];

            if (IntPtr.Size == 4)
            {
                parameters[0] = count;
                parameters[1] = pairs;
            }
            else
            {
                parameters[0] = count;
                var longPairs = new ulong[pairs.Length];
                for (var i = 0; i < pairs.Length; i++) { longPairs[i] = pairs[i]; }
                parameters[1] = longPairs;
            }

            method.Invoke(control, parameters);
        }
    }
}
