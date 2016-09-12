using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Arcade.Models;
using Arcade.Utilities;
using Arcade.ViewModels;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.RemoteWindows;
using Siune.Client;

namespace Arcade.Tasks
{
    internal static class Behaviors
    {
        public static DateTime TimeToSwitchGame;
        public static string CurrentGame = "None";

        private const string
            Cuff = "Cuff-a-Cur",
            Monster = "Monster Toss",
            Crystal = "Crystal Tower Stryker",
            Moogles = "Moogles Paw";

        public static async Task<bool> Main()
        {
            if (!SiuneSession.IsAuthenticated(18))
                return false;

            if (!GoldenSaucer.InZone)
            {
                return await GoldenSaucer.TeleportToSaucer();
            }

            Mgp.UpdateTimeSpan();

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
                    Logging.Write(Colors.Red, $@"[Arcade] Stopped because we have reached MGP limit.");
                    TreeRoot.Stop(" Stopped because we have reached MGP limit.");
                    return false;
                }
            }

            if (Settings.Instance.JumboCactpot)
            {
                if (await JumboCactpot.BuyTicket()) return true;
                if (await JumboCactpot.CollectReward()) return true;
            }

            if (DateTime.Now < TimeToSwitchGame)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (CurrentGame)
                {
                    case Cuff:
                        return await CuffACur.Play();
                    case Monster:
                        return await MonsterToss.Play();
                    case Crystal:
                        return await CrystalTowerStryker.Play();
                    case Moogles:
                        return await MooglesPaw.Play();
                }
            }

            var gamesList = GenerateGamesList;
            var count = gamesList.Count;

            if (count < 2)
            {
                if (Settings.Instance.CuffACurr && await CuffACur.Play()) return true;
                if (Settings.Instance.MonsterToss && await MonsterToss.Play()) return true;
                if (Settings.Instance.CrystalTowerStryker && await CrystalTowerStryker.Play()) return true;
                return Settings.Instance.MooglesPaw && await MooglesPaw.Play();
            }

            var randomIndex = new Random().Next(count);
            var newGame = gamesList[randomIndex];

            Logging.Write(Colors.DodgerBlue, $@"[Arcade] Picking {newGame} as our next game.");

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (newGame)
            {
                case Cuff:
                    CurrentGame = Cuff;
                    TimeToSwitchGame = DateTime.Now.AddMinutes(new Random().Next(Settings.Instance.MinMinutes, Settings.Instance.MaxMinutes));
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Switching games again at {TimeToSwitchGame.ToShortTimeString()}.");
                    return await CuffACur.Play();

                case Monster:
                    CurrentGame = Monster;
                    TimeToSwitchGame = DateTime.Now.AddMinutes(new Random().Next(Settings.Instance.MinMinutes, Settings.Instance.MaxMinutes));
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Switching games again at {TimeToSwitchGame.ToShortTimeString()}.");
                    return await MonsterToss.Play();

                case Crystal:
                    CurrentGame = Crystal;
                    TimeToSwitchGame = DateTime.Now.AddMinutes(new Random().Next(Settings.Instance.MinMinutes, Settings.Instance.MaxMinutes));
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Switching games again at {TimeToSwitchGame.ToShortTimeString()}.");
                    return await CrystalTowerStryker.Play();

                case Moogles:
                    CurrentGame = Moogles;
                    TimeToSwitchGame = DateTime.Now.AddMinutes(new Random().Next(Settings.Instance.MinMinutes, Settings.Instance.MaxMinutes));
                    Logging.Write(Colors.DodgerBlue, $@"[Arcade] Switching games again at {TimeToSwitchGame.ToShortTimeString()}.");
                    return await MooglesPaw.Play();
            }

            return true;
        }

        private static List<string> GenerateGamesList
        {
            get
            {
                var list = new List<string>();

                if (Settings.Instance.CuffACurr && CurrentGame != Cuff)
                    list.Add(Cuff);

                if (Settings.Instance.MonsterToss && CurrentGame != Monster)
                    list.Add(Monster);

                if (Settings.Instance.CrystalTowerStryker && CurrentGame != Crystal)
                    list.Add(Crystal);

                if (Settings.Instance.MooglesPaw && CurrentGame != Moogles)
                    list.Add(Moogles);

                return list;
            }
        }
    }
}
