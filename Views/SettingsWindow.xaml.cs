using System.Windows;
using System.Windows.Input;
using Arcade.Models;
using Siune.Client;

namespace Arcade.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            if (!SiuneSession.IsAuthenticated(24))
            {
                Arcade.Login.ShowDialog();
            }
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.Instance.Save();
            Close();
        }
    }
}
