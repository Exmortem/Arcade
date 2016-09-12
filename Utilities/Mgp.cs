using System;
using System.Text.RegularExpressions;
using System.Windows;
using Arcade.ViewModels;
using ff14bot.Managers;

namespace Arcade.Utilities
{
    public static class Mgp
    {
        public static DateTime StartTime;
        public static bool IsRunning;
        private static readonly Regex MgpRegex = new Regex(@"You\sobtain\s([0-9]*)\sMGP");
        private static readonly Regex CnMgpRegex = new Regex(@"了([0-9]*)金");

        public static void MessageReceived(object sender, ChatEventArgs chatEventArgs)
        {
            var match = MgpRegex.Match(chatEventArgs.ChatLogEntry.Contents);

            if (!match.Success)
            {
                match = CnMgpRegex.Match(chatEventArgs.ChatLogEntry.Contents);

                if (!match.Success)
                    return;
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                ArcadeViewModel.Instance.GamesPlayed++;ArcadeViewModel.Instance.MgpGained = ArcadeViewModel.Instance.MgpGained + int.Parse(match.Groups[1].Value);
                ArcadeViewModel.Instance.MgpPerHour = (int)(ArcadeViewModel.Instance.MgpGained/(DateTime.Now - StartTime).TotalHours);
            });
        }

        public static void UpdateTimeSpan()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                var timeSpan = (DateTime.Now - StartTime);
                ArcadeViewModel.Instance.RunningTime = new TimeSpan(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds).ToString();
            });
        }
    }
}
