using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Arcade.Languages;
using Arcade.Models;
using Arcade.Utilities;
using Arcade.ViewModels;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using AuthCoreClient;

namespace Arcade.Tasks
{
    internal static class Behaviors
    {
        static Behaviors()
        {
            Cuff = Language.Instance.Cuff;
            Monster = Language.Instance.Monster;
            Crystal = Language.Instance.Crystal;
            Moogles = Language.Instance.Moogles;
        }

        public static DateTime TimeToSwitchGame;
        public static bool SwitchedGame;
        public static bool PlayingOnlyOneGame;
        public static string CurrentGame = "None";

        public static readonly string Cuff;
        public static readonly string Monster;
        public static readonly string Crystal;
        public static readonly string Moogles;

        public static async Task<bool> Main()
        {
            if (!AuthCoreSession.IsAuthenticated(2))
                return false;

            if (!GoldenSaucer.InZone)
            {
                return await GoldenSaucer.TeleportToSaucer();
            }

            Mgp.UpdateTimeSpan();
            Mgp.UpdateStats();

            if (Talk.DialogOpen)
            {
                Talk.Next();
                return true;
            }

            if (Settings.Instance.StopIfReachCertainMgp)
            {
                if (ArcadeViewModel.Instance.MgpGained >= Settings.Instance.MgpStopPoint)
                {
                    GamelogManager.MessageRecevied -= Mgp.MessageReceived;
                    Logging.Write(Colors.Red, Language.Instance.LogStopMgpLimit);
                    TreeRoot.Stop();
                    return false;
                }
            }

            if (await MiniCactpot.Play()) return true;

            //if (Settings.Instance.JumboCactpot)
            //{
            //    if (await JumboCactpot.BuyTicket()) return true;
            //    if (await JumboCactpot.CollectReward()) return true;
            //}

            if (DateTime.Now < TimeToSwitchGame)
            {
                if (CurrentGame == Cuff)
                {
                    return await CuffACur.Play();
                }

                if (CurrentGame == Monster)
                {
                    return await MonsterToss.Play();
                }

                if (CurrentGame == Crystal)
                {
                    return await CrystalTowerStryker.Play();
                }

                if (CurrentGame == Moogles)
                {
                    return await MooglesPaw.Play();
                }
            }

            var gamesList = GenerateGamesList;
            var count = gamesList.Count;
            var randomIndex = new Random().Next(count);

            if (count == 1)
            {
                PlayingOnlyOneGame = true;
                if (Settings.Instance.CuffACurr && await CuffACur.Play()) return true;
                if (Settings.Instance.CrystalTowerStryker && await CrystalTowerStryker.Play()) return true;
                if (Settings.Instance.MonsterToss && await MonsterToss.Play()) return true;
                if (Settings.Instance.MooglesPaw && await MooglesPaw.Play()) return true;
            }

            if (count == 0)
            {
                return false;
            }

            var newGame = count == 0 ? CurrentGame : gamesList[randomIndex];

            Logging.Write(Colors.DodgerBlue, $@"{Language.Instance.LogPickingGame} {newGame}");
            TimeToSwitchGame = DateTime.Now.AddMinutes(new Random().Next(Settings.Instance.MinMinutes, Settings.Instance.MaxMinutes));
            SwitchedGame = true;
            Logging.Write(Colors.DodgerBlue, $@"{Language.Instance.LogSwitchingGamesAgainAt} {TimeToSwitchGame:HH:mm:ss}.");

            if (newGame == Cuff)
            {
                CurrentGame = Cuff;
                return await CuffACur.Play();
            }

            if (newGame == Monster)
            {
                CurrentGame = Monster;
                return await MonsterToss.Play();
            }

            if (newGame == Crystal)
            {
                CurrentGame = Crystal;
                return await CrystalTowerStryker.Play();
            }

            if (newGame != Moogles)
                return true;

            CurrentGame = Moogles;
            return await MooglesPaw.Play();
        }
        
        private static List<string> GenerateGamesList
        {
            get
            {
                var list = new List<string>();

                if (Settings.Instance.CuffACurr)
                    list.Add(Cuff);

                if (Settings.Instance.MonsterToss)
                    list.Add(Monster);

                if (Settings.Instance.CrystalTowerStryker)
                    list.Add(Crystal);

                if (Settings.Instance.MooglesPaw)
                    list.Add(Moogles);

                return list;
            }
        }
    }
}
