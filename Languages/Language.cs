using System;
using System.Text.RegularExpressions;

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
        public string ArcadeLogin
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Arcade: Login";
                    case Languages.German:
                        return "Arcade: Login";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游乐场：请登录";
                    default:
                        return "Arcade: Login";
                }
            }
        }

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
                        return "Arcade: MGP Farmer";
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
                        return "Spiele Hau-den-Gilli";
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
                        return "Spiele Kaktor-Katapult";
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
                        return "Spiele Bomberball";
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
                        return "Spiele Mogry-Greifer";
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
                        return "Minimale Minuten pro Spiel";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "最小游戏时间（分钟）";
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
                        return "Maximale Minuten pro Spiel";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "最大游戏时间（分钟）";
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
                        return "Spiele Jumbo-Glückskaktor";
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
                        return "Schneller Modus (Spiele so schnell wie möglich)";
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
                        return "Nach gesammelten MGP stoppen: ";
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
                        return "MGP gesammelt: ";
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
                        return "MGP pro Stunde: ";
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
                        return "Spiele gespielt: ";
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
                        return "Laufzeit: ";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "游戏时间总计 ";
                    default:
                        return "Running Time: ";
                }
            }
        }

        public string PlayMiniCactpot
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "Play Mini Cactpot";
                    case Languages.English:
                        return "Play Mini Cactpot";
                    case Languages.German:
                        return "Spiele Mini-Glückskaktor";
                    case Languages.French:
                        return "Play Mini Cactpot";
                    case Languages.Chinese:
                        return "购买仙人微彩";
                    default:
                        return "Play Mini Cactpot";
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
                        return "Hau-den-Gilli";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "重击伽美什";
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
                        return "Bomberball";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "怪物投篮";
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
                        return "Kaktor-Katapult";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "强袭水晶塔";
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
                        return "Mogry-Greifer";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "莫古抓球机";
                    default:
                        return "Moogles Paw";
                }
            }
        }
        #endregion

        #region Log

        //停止工作，尚未解锁金碟游乐场。

        public string LogStoppingNoAetheryte
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] Stopping bot because you do not have the Gold Saucer aetheryte unlocked.";
                    case Languages.German:
                        return "[Arcade] Der Bot wird gestoppt, da du den Gold Saucer Ätheryten nicht freigeschalten hast.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "停止工作，尚未解锁金碟游乐场。";
                    default:
                        return "Next game:";
                }
            }
        }

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
                        return "[Arcade] Nächstes Spiel:";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "下一个游戏：";
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
                        return "[Arcade] Angehalten, da wir das MGP Limit erreicht haben.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "停止游戏，收益目标达成！";
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
                        return "[Arcade] Ändere Spiel um";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "更换游戏时间为";
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
                        return "[Arcade] Einstellungen geändert, wir wollen kein Hau-den-Gilli mehr spielen.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "设置变更，不再玩“重击伽美什”。";
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
                        return "[Arcade] Einstellungen geändert, wir wollen kein Bomberball mehr spielen.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "设置变更，不再玩“怪物投篮”。";
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
                        return "[Arcade] Einstellungen geändert, wir wollen kein Kaktor-Katapult mehr spielen.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "设置变更，不再玩“强袭水晶塔”。";
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
                        return "[Arcade] Einstellungen geändert, wir wollen kein Mogry-Greifer mehr spielen.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "设置变更，不再玩“莫古抓球机”。";
                    default:
                        return "[Arcade] Settings changed, we do not want to play Moogles Paw anymore.";
                }
            }
        }
        #endregion

        #region Cactpot
        public string CactpotNotTimeToDrawYet
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] It is not time to draw yet, or we don't have a ticket.";
                    case Languages.German:
                        return "[Arcade] Es ist noch nicht Zeit zur Ziehung oder wir haben kein Ticket.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "重击伽美什";
                    default:
                        return "[Arcade] It is not time to draw yet, or we don't have a ticket.";
                }
            }
        }

        public string CactpotHaveNotBoughtTicketYet
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] We have not bought a ticket yet.";
                    case Languages.German:
                        return "[Arcade] Wir haben noch kein Ticket gekauft.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "重击伽美什";
                    default:
                        return "[Arcade] We have not bought a ticket yet.";
                }
            }
        }

        public string CactpotAlreadyHaveTicket
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "[Arcade] We already have a Jumbo Cactpot ticket.";
                    case Languages.German:
                        return "[Arcade] Wir haben schon ein Jumbo-Glückskaktor Ticket.";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "重击伽美什";
                    default:
                        return "[Arcade] We already have a Jumbo Cactpot ticket.";
                }
            }
        }

        public string CactpotPayout
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "payout";
                    case Languages.German:
                        return "Den Teilnahmebonus überprüfen";
                    case Languages.French:
                        return "l'état du bonus";
                    case Languages.Chinese:
                        return "查看购";
                    default:
                        return "payout";
                }
            }
        }

        public string MiniCactpotPurchaseTicket
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return "";
                    case Languages.English:
                        return "Purchase";
                    case Languages.German:
                        return "Ein";
                    case Languages.French:
                        return "";
                    case Languages.Chinese:
                        return "参加仙人微彩有奖竞猜";
                    default:
                        return "Purchase";
                }
            }
        }
        #endregion

        #region Regex
        public Regex MgpRegex
        {
            get
            {
                switch (ClientLanguage)
                {
                    case Languages.Japanese:
                        return new Regex("");
                    case Languages.English:
                        return new Regex(@"You\sobtain\s([0-9]*)\sMGP");
                    case Languages.German:
                        return new Regex("");
                    case Languages.French:
                        return new Regex("");
                    case Languages.Chinese:
                        return new Regex(@"了([0-9]*)金");
                    default:
                        return new Regex(@"You\sobtain\s([0-9]*)\sMGP");
                }
            }
        }
        #endregion
    }
}
