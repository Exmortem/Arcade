using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Arcade.Models;

namespace Arcade.ViewModels
{
    public class ArcadeViewModel : INotifyPropertyChanged
    {
        private static ArcadeViewModel _instance;
        public static ArcadeViewModel Instance => _instance ?? (_instance = new ArcadeViewModel());

        public static Settings Settings => Settings.Instance;

        private int _mgpGained;
        private int _mgpPerHour;
        private int _gamesPlayed;
        private string _runningTime;

        public int MgpGained
        {
            get { return _mgpGained; }
            set
            {
                _mgpGained = value;
                OnPropertyChanged();
            }
        }

        public int MgpPerHour
        {
            get { return _mgpPerHour; }
            set
            {
                _mgpPerHour = value;
                OnPropertyChanged();
            }
        }

        public int GamesPlayed
        {
            get { return _gamesPlayed; }
            set
            {
                _gamesPlayed = value;
                OnPropertyChanged();
            }
        }

        public string RunningTime
        {
            get { return _runningTime; }
            set
            {
                _runningTime = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
