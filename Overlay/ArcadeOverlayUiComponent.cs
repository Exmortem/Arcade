using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Arcade.Properties;
using Arcade.ViewModels;
using Arcade.Views.UserControls;
using Buddy.Overlay;
using Buddy.Overlay.Controls;
using Settings = Arcade.Models.Settings;

namespace Arcade.Overlay
{
    internal class ArcadeOverlayUiComponent : OverlayUIComponent
    {
        public ArcadeOverlayUiComponent(bool isHitTestable) : base(true)  { }

        private OverlayControl _control;

        public override OverlayControl Control
        {
            get
            {
                if (_control != null)
                    return _control;

                //var button = new Button { Content = "This is a Button" };
                //button.Click += (sender, args) => button.Content = "Clicked!";

                var border = new Border() { CornerRadius = new CornerRadius(5), Background = new SolidColorBrush(Colors.Black), Opacity = 0.8, Padding = new Thickness(10)};
                var resourceDictionary = Application.LoadComponent(new Uri("/Arcade;component/Styles/Arcade.xaml", UriKind.Relative)) as ResourceDictionary;
                border.Resources.MergedDictionaries.Add(resourceDictionary);

                var grid = new Grid() { DataContext = ArcadeViewModel.Instance };

                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                var arcadeText = new TextBlock() { FontSize = 14, Foreground = new SolidColorBrush(Colors.MediumSeaGreen), HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0,0,0,2) };
                var arcadeTextBinding = new Binding("Languages.Arcade");
                BindingOperations.SetBinding(arcadeText, TextBlock.TextProperty, arcadeTextBinding);

                var mgpGainedSoFarNumber = new TextBlock() { FontSize = 12, Foreground = Brushes.White, Padding = new Thickness(5, 0, 0, 0) };
                var mgpGainedSoFarNumberBinding = new Binding("MgpGained");
                BindingOperations.SetBinding(mgpGainedSoFarNumber, TextBlock.TextProperty, mgpGainedSoFarNumberBinding);

                var mgpGainedSoFarText = new TextBlock() { FontSize = 12, Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right};
                var mgpGainedSoFarTextBinding = new Binding("Languages.MgpGainedSoFar");
                BindingOperations.SetBinding(mgpGainedSoFarText, TextBlock.TextProperty, mgpGainedSoFarTextBinding);

                var mgpPerHourNumber = new TextBlock() { FontSize = 12, Foreground = Brushes.White, Padding = new Thickness(5, 0, 0, 0) };
                var mgpPerHourNumberBinding = new Binding("MgpPerHour");
                BindingOperations.SetBinding(mgpPerHourNumber, TextBlock.TextProperty, mgpPerHourNumberBinding);

                var mgpPerHourText = new TextBlock() { FontSize = 12, Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right };
                var mgpPerHourTextBinding = new Binding("Languages.MgpPerHour");
                BindingOperations.SetBinding(mgpPerHourText, TextBlock.TextProperty, mgpPerHourTextBinding);

                var gamesPlayNumber = new TextBlock() { FontSize = 12, Foreground = Brushes.White, Padding = new Thickness(5, 0, 0, 0) };
                var gamesPlayNumberBinding = new Binding("GamesPlayed");
                BindingOperations.SetBinding(gamesPlayNumber, TextBlock.TextProperty, gamesPlayNumberBinding);

                var gamesPlayText = new TextBlock() { FontSize = 12, Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right };
                var gamesPlayTextBinding = new Binding("Languages.GamesPlayed");
                BindingOperations.SetBinding(gamesPlayText, TextBlock.TextProperty, gamesPlayTextBinding);

                var runningTimeNumber = new TextBlock() { FontSize = 12, Foreground = Brushes.White, Padding = new Thickness(5, 0, 0, 0) };
                var runningTimeBinding = new Binding("RunningTime");
                BindingOperations.SetBinding(runningTimeNumber, TextBlock.TextProperty, runningTimeBinding);

                var runningTimeText = new TextBlock() { FontSize = 12, Foreground = Brushes.White, HorizontalAlignment = HorizontalAlignment.Right };
                var runningTimeTextBinding = new Binding("Languages.RunningTime");
                BindingOperations.SetBinding(runningTimeText, TextBlock.TextProperty, runningTimeTextBinding);

                Grid.SetRow(arcadeText, 0);
                Grid.SetColumn(arcadeText, 0);
                Grid.SetColumnSpan(arcadeText, 2);

                Grid.SetRow(mgpGainedSoFarText, 1);
                Grid.SetRow(mgpGainedSoFarNumber, 1);
                Grid.SetColumn(mgpGainedSoFarText, 0);
                Grid.SetColumn(mgpGainedSoFarNumber, 1);

                Grid.SetRow(mgpPerHourText, 2);
                Grid.SetRow(mgpPerHourNumber, 2);
                Grid.SetColumn(mgpPerHourText, 0);
                Grid.SetColumn(mgpPerHourNumber, 1);

                Grid.SetRow(gamesPlayText, 3);
                Grid.SetRow(gamesPlayNumber, 3);
                Grid.SetColumn(gamesPlayText, 0);
                Grid.SetColumn(gamesPlayNumber, 1);

                Grid.SetRow(runningTimeText, 4);
                Grid.SetRow(runningTimeNumber, 4);
                Grid.SetColumn(runningTimeText, 0);
                Grid.SetColumn(runningTimeNumber, 1);

                //var testControl = new testControl();
                //Grid.SetRow(testControl, 5);
                //Grid.SetColumn(testControl, 0);
                //Grid.SetColumnSpan(testControl, 2);
                //grid.Children.Add(testControl);

                grid.Children.Add(arcadeText);

                grid.Children.Add(mgpGainedSoFarText);
                grid.Children.Add(mgpGainedSoFarNumber);

                grid.Children.Add(mgpPerHourText);
                grid.Children.Add(mgpPerHourNumber);

                grid.Children.Add(gamesPlayText);
                grid.Children.Add(gamesPlayNumber);

                grid.Children.Add(runningTimeText);
                grid.Children.Add(runningTimeNumber);

                border.Child = grid;

                _control = new OverlayControl
                {
                    Name = "ArcadeOverlay",
                    Content = border,
                    X = Settings.Instance.OverlayPosX,
                    Y = Settings.Instance.OverlayPosY,
                    AllowMoving = true,           
                };

                _control.MouseLeave += (sender, args) =>
                {
                    Settings.Instance.OverlayPosX = _control.X;
                    Settings.Instance.OverlayPosY = _control.Y;
                    Settings.Instance.Save();
                }; 

                return _control;
            }
        }
    }
}
