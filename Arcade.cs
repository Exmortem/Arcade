using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Arcade.Languages;
using Arcade.Models;
using Arcade.Tasks;
using Arcade.Utilities;
using Arcade.ViewModels;
using Arcade.Views;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using Siune.Client;
using TreeSharp;
using LoginWindow = Arcade.Views.LoginWindow;

namespace Arcade
{
    public class Arcade
    {
        public Arcade()
        {
            SiuneSession.RegisterProduct(18, "Arcade", Log);
            SiuneSession.SetProduct(18, Settings.Instance.Key);
            ArcadeViewModel.Instance.RunningTime = "00:00:00";

            //var patternFinder = new GreyMagic.PatternFinder(Core.Memory);
            //var intPtr = patternFinder.Find("Search 56 8B F1 75 ?? 83 0D ?? ?? ?? ?? 01 6A FF B9 ?? ?? ?? ?? Add F Read32");
            //var languageByte = Core.Memory.Read<byte>(intPtr);

            int languageByte;

            var sprint = Actionmanager.CurrentActions.Values.FirstOrDefault(r => r.Id == 3);

            if (sprint == null)
            {
                languageByte = 1;
            }
            else
            {
                switch (sprint.LocalizedName)
                {
                    case "Sprint":
                        languageByte = 1;
                        break;
                    case "冲刺":
                        languageByte = 4;
                        break;
                    default:
                        languageByte = 1;
                        break;
                }
            }

            //Language.Instance.ClientLanguage = Languages.Languages.Chinese;

            switch (languageByte)
            {
                case 0:
                    Language.Instance.ClientLanguage = Languages.Languages.Japanese;
                    break;
                case 1:
                    Language.Instance.ClientLanguage = Languages.Languages.English;
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Language: English");
                    break;
                case 2:
                    Language.Instance.ClientLanguage = Languages.Languages.German;
                    break;
                case 3:
                    Language.Instance.ClientLanguage = Languages.Languages.French;
                    break;
                case 4:
                    Language.Instance.ClientLanguage = Languages.Languages.Chinese;
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Language: Chinese");
                    break;
                default:
                    Language.Instance.ClientLanguage = Languages.Languages.English;
                    break;
            }
        }

        private static void Log(string text)
        {
            Logging.Write(Colors.DodgerBlue, $@"[Arcade] {text}");
        }

        public void Start()
        {
            if (Navigator.PlayerMover == null)
            {
                Navigator.PlayerMover = new SlideMover();
            }

            if (Navigator.NavigationProvider == null)
            {
                Navigator.NavigationProvider = new GaiaNavigator();
            }

            GamelogManager.MessageRecevied += Mgp.MessageReceived;
            Mgp.StartTime = DateTime.Now;
            Mgp.IsRunning = true;
            Behaviors.CurrentGame = "None";
            Behaviors.TimeToSwitchGame = DateTime.Now;

            Application.Current.Dispatcher.Invoke(delegate
            {
                ArcadeViewModel.Instance.MgpGained = 0;
                ArcadeViewModel.Instance.GamesPlayed = 0;
                ArcadeViewModel.Instance.MgpPerHour = 0;
            });
        }

        public void Stop()
        {
            Mgp.IsRunning = false;
            GamelogManager.MessageRecevied -= Mgp.MessageReceived;
        }

        private Composite _root;

        public Composite GetRoot()
        {
            return _root ?? (_root = new ActionRunCoroutine(r => Behaviors.Main()));
        }

        #region Form
        public static void OnButtonPress()
        {
            if (Form.IsVisible)
                return;

            Form.Show();
        }

        private static SettingsWindow _form;

        private static SettingsWindow Form
        {
            get
            {
                if (_form != null) return _form;
                _form = new SettingsWindow();
                _form.Closed += (sender, args) => _form = null;
                return _form;
            }
        }

        private static LoginWindow _login;

        public static LoginWindow Login
        {
            get
            {
                if (_login != null) return _login;
                _login = new LoginWindow();
                _login.Closed += (sender, args) => _login = null;
                return _login;
            }
        }
        #endregion
    }
}
