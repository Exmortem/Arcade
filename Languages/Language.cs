using System;

namespace Arcade.Languages
{
    public enum Languages
    {
        Japanese,
        English,
        German,
        French,
        Chinese
    }

    public class Language
    {
        private static Language _instance;
        public static Language Instance => _instance ?? (_instance = new Language());

        public Languages ClientLanguage;

        #region GUI
        public string Arcade
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Arcade: MGP Farmer";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Arcade: MGP Farmer"; 
                }
            }
        }

        public string PlayCuffaCurr
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Play Cuff-a-Curr";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "重击伽美什";
                    default:
                        return "Play Cuff-a-Curr";
                }
            }
        }

        public string PlayCrystalTowerStryker
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Play Crystal Tower Stryker";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "强袭水晶塔";
                    default:
                        return "Play Crystal Tower Stryker";
                }
            }
        }

        public string PlayMonsterToss
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Play Monster Toss";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "怪物投篮";
                    default:
                        return "Play Monster Toss";
                }
            }
        }

        public string PlayMooglesPaw
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Play Moogles Paw";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "莫古抓球机";
                    default:
                        return "Play Moogles Paw";
                }
            }
        }

        public string MinimumMinutesPlayingGame
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Minimum Minutes Playing Game";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "最小游戏时间";
                    default:
                        return "Minimum Minutes Playing Game";
                }
            }
        }

        public string MaximumMinutesPlayingGame
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Maximum Minutes Playing Game";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "最大游戏时间 ";
                    default:
                        return "Maximum Minutes Playing Game";
                }
            }
        }

        public string PlayJumboCactpot
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Play Jumbo Cactpot";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "购买仙人仙彩";
                    default:
                        return "Play Jumbo Cactpot";
                }
            }
        }

        public string FastMode
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Fast Mode (Play as Fast as Possible)";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "快速模式";
                    default:
                        return "Fast Mode (Play as Fast as Possible)";
                }
            }
        }

        public string StopAfterGettingMgp
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Stop After Getting MGP: ";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "设置收益目标 ";
                    default:
                        return "Stop After Getting MGP: ";
                }
            }
        }

        public string MgpGainedSoFar
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "MGP Gained So Far: ";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "收益统计 ";
                    default:
                        return "MGP Gained So Far: ";
                }
            }
        }

        public string MgpPerHour
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "MGP Per Hour: ";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "每小时收益 ";
                    default:
                        return "MGP Per Hour: ";
                }
            }
        }

        public string GamesPlayed
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Games Played: ";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游戏回合总数 ";
                    default:
                        return "Games Played: ";
                }
            }
        }

        public string RunningTime
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Running Time: ";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游戏时间总计 ";
                    default:
                        return "Running Time: ";
                }
            }
        }
        #endregion

        #region Games
        public string Cuff
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Cuff-a-Cur";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Cuff-a-Cur";
                }
            }
        }

        public string Monster
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Monster Toss";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Monster Toss";
                }
            }
        }

        public string Crystal
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Crystal Tower Stryker";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Crystal Tower Stryker";
                }
            }
        }

        public string Moogles
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Moogles Paw";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Moogles Paw";
                }
            }
        }
        #endregion

        #region Log
        public string LogPickingGame
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Next game:";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "Next game:";
                }
            }
        }

        public string LogStopMgpLimit
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Stopped because we have reached MGP limit.";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Stopped because we have reached MGP limit.";
                }
            }
        }

        public string LogSwitchingGamesAgainAt
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Switching games again at";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Switching games again at";
                }
            }
        }

        public string LogDontPlayCuffAnymore
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Settings changed, we do not want to play Cuff-a-Cur anymore.";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Settings changed, we do not want to play Cuff-a-Cur anymore.";
                }
            }
        }

        public string LogDontPlayMonsterAnymore
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Settings changed, we do not want to play Monster Toss anymore.";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Settings changed, we do not want to play Monster Toss anymore.";
                }
            }
        }

        public string LogDontPlayCrystalAnymore
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Settings changed, we do not want to play Crystal Tower Stryker anymore.";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Settings changed, we do not want to play Crystal Tower Stryker anymore.";
                }
            }
        }

        public string LogDontPlayMoogleAnymore
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Settings changed, we do not want to play Moogles Paw anymore.";
                    case Languages.German:
                        return "";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：金碟币快手";
                    default:
                        return "[Arcade] Settings changed, we do not want to play Moogles Paw anymore.";
                }
            }
        }
        #endregion
    }
}
