using System;
using System.Text.RegularExpressions;
using System.Windows;
using Arcade.Languages;
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
        private static readonly Regex GermanMgpRegex = new Regex(@"Du\shast\s([0-9]*)\sMGP");

        public static void MessageReceived(object sender, ChatEventArgs chatEventArgs)
        {
            var contents = chatEventArgs.ChatLogEntry.Contents.Replace(",", "");

            Match match;

            switch (Language.Instance.ClientLanguage)
            {
                case Languages.Languages.Japanese:
                    match = MgpRegex.Match(contents);
                    break;
                case Languages.Languages.English:
                    match = MgpRegex.Match(contents);
                    break;
                case Languages.Languages.German:
                    match = GermanMgpRegex.Match(contents);
                    break;
                case Languages.Languages.French:
                    match = MgpRegex.Match(contents);
                    break;
                case Languages.Languages.Chinese:
                    match = CnMgpRegex.Match(contents);
                    break;
                default:
                    match = MgpRegex.Match(contents);
                    break;
            }

            if (!match.Success)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                ArcadeViewModel.Instance.GamesPlayed++;
                ArcadeViewModel.Instance.MgpGained = ArcadeViewModel.Instance.MgpGained + int.Parse(match.Groups[1].Value);
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
