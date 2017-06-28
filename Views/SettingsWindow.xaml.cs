using System.Windows;
using System.Windows.Input;
using Arcade.Models;
using AuthCoreClient;

namespace Arcade.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            if (!AuthCoreSession.IsAuthenticated(2))
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
