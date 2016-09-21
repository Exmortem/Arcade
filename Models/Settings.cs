using System;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using Arcade.Overlay;
using Arcade.Tasks;
using Arcade.Utilities;
using ff14bot;
using ff14bot.Helpers;

namespace Arcade.Models
{
    public class Settings : JsonSettings, INotifyPropertyChanged
    {
        private static Settings _instance;
        public static Settings Instance => _instance ?? (_instance = new Settings());

        private Settings() : base(CharacterSettingsDirectory + "/Arcade/Arcade.json") { }

        private string _key;
        private string _email;

        [Setting]
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private bool _useOverlay;

        [Setting]
        [DefaultValue(false)]
        public bool UseOverlay
        {
            get { return _useOverlay; }
            set
            {
                _useOverlay = value;

                if (value == false)
                {
                    //ArcadeOverlay.Stop();
                }
                else
                {
                    //if (TreeRoot.IsRunning)
                    //{
                    //    ArcadeOverlay.Start();
                    //}
                }
                
                OnPropertyChanged();
            }
        }

        private double _overlayPosX;
        private double _overlayPosY;

        [Setting]
        [DefaultValue(60)]
        public double OverlayPosX
        {
            get { return _overlayPosX; }
            set
            {
                _overlayPosX = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(60)]
        public double OverlayPosY
        {
            get { return _overlayPosY; }
            set
            {
                _overlayPosY = value;
                OnPropertyChanged();
            }
        }

        private bool _miniCactpot;
        private bool _miniCactpotPlayedToday;

        [Setting]
        [DefaultValue(false)]
        public bool MiniCactpot
        {
            get { return _miniCactpot; }
            set
            {
                _miniCactpot = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(false)]
        public bool MiniCactpotPlayedToday
        {
            get { return _miniCactpotPlayedToday; }
            set
            {
                _miniCactpotPlayedToday = value;
                OnPropertyChanged();
            }
        }

        private bool _jumboCactpot;
        private bool _jumboCactpotBoughtTicket;
        private DateTime _jumboCactpotBuyTime;

        [Setting]
        [DefaultValue(false)]
        public bool JumboCactpot
        {
            get { return _jumboCactpot; }
            set
            {
                _jumboCactpot = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(false)]
        public bool JumboCactpotBoughtTicket
        {
            get { return _jumboCactpotBoughtTicket; }
            set
            {
                _jumboCactpotBoughtTicket = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        public DateTime JumboCactpotBuyTime
        {
            get { return _jumboCactpotBuyTime; }
            set
            {
                _jumboCactpotBuyTime = value;
                OnPropertyChanged();
            }
        }

       private int _minMinutes;
        private int _maxMinutes;

        [Setting]
        [DefaultValue(5)]
        public int MinMinutes
        {
            get { return _minMinutes; }
            set
            {
                _minMinutes = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(10)]
        public int MaxMinutes
        {
            get { return _maxMinutes; }
            set
            {
                _maxMinutes = value;
                OnPropertyChanged();
            }
        }

        private bool _fastMode;

        [Setting]
        [DefaultValue(false)]
        public bool FastMode
        {
            get { return _fastMode; }
            set
            {
                _fastMode = value;
                OnPropertyChanged();
            }
        }

        private bool _randomizeScores;

        [Setting]
        [DefaultValue(true)]
        public bool RandomizeScores
        {
            get { return _randomizeScores; }
            set
            {
                _randomizeScores = value;
                OnPropertyChanged();
            }
        }

        private bool _cuffACurr;
        private bool _monsterToss;
        private bool _crystalTowerStryker;
        private bool _mooglesPaw;
        private bool _stopIfReachCertainMgp;
        private int _mgpStopPoint;

        [Setting]
        [DefaultValue(true)]
        public bool CuffACurr
        {
            get { return _cuffACurr; }
            set
            {
                _cuffACurr = value;

                if (value == false && Mgp.IsRunning && Behaviors.CurrentGame == Behaviors.Cuff)
                {
                    Behaviors.TimeToSwitchGame = DateTime.Now;
                    //Logging.Write(Colors.DodgerBlue, Language.Instance.LogDontPlayCuffAnymore);
                }

                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(true)]
        public bool MonsterToss
        {
            get { return _monsterToss; }
            set
            {
                _monsterToss = value;

                if (value == false && Mgp.IsRunning && Behaviors.CurrentGame == Behaviors.Monster)
                {
                    Behaviors.TimeToSwitchGame = DateTime.Now;
                    //Logging.Write(Colors.DodgerBlue, Language.Instance.LogDontPlayMonsterAnymore);
                }

                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(true)]
        public bool CrystalTowerStryker
        {
            get { return _crystalTowerStryker; }
            set
            {
                _crystalTowerStryker = value;

                if (value == false && Mgp.IsRunning && Behaviors.CurrentGame == Behaviors.Crystal)
                {
                    Behaviors.TimeToSwitchGame = DateTime.Now;
                    //Logging.Write(Colors.DodgerBlue, Language.Instance.LogDontPlayCrystalAnymore);
                }

                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(true)]
        public bool MooglesPaw
        {
            get { return _mooglesPaw; }
            set
            {
                _mooglesPaw = value;

                if (value == false && Mgp.IsRunning && Behaviors.CurrentGame == Behaviors.Moogles)
                {
                    Behaviors.TimeToSwitchGame = DateTime.Now;
                    //Logging.Write(Colors.DodgerBlue, Language.Instance.LogDontPlayMoogleAnymore);
                }

                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(false)]
        public bool StopIfReachCertainMgp
        {
            get { return _stopIfReachCertainMgp; }
            set
            {
                _stopIfReachCertainMgp = value;
                OnPropertyChanged();
            }
        }

        [Setting]
        [DefaultValue(150000)]
        public int MgpStopPoint
        {
            get { return _mgpStopPoint; }
            set
            {
                _mgpStopPoint = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
