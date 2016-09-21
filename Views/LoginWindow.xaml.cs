using System.Windows;
using System.Windows.Input;
using Arcade.Models;
using Siune.Client;

namespace Arcade.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CheckAuth(object sender, RoutedEventArgs e)
        {
            CheckAuth();
        }

        private void CheckAuth()
        {
            if (string.IsNullOrEmpty(TxtKey.Text) || string.IsNullOrEmpty(TxtEmail.Text))
                return;
            
            Settings.Instance.Key = TxtKey.Text.Trim();
            Settings.Instance.Email = TxtEmail.Text.Trim();
            SiuneSession.SetProduct(24, Settings.Instance.Key);
            Settings.Instance.Save();
        }

        private void TestKey(object sender, RoutedEventArgs e)
        {
            TxtAuth.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(TxtKey.Text) || string.IsNullOrEmpty(TxtEmail.Text))
            {
                TxtAuth.Text = "More Information Needed";
                return;
            }

            Settings.Instance.Key = TxtKey.Text.Trim();
            Settings.Instance.Email = TxtEmail.Text.Trim();
            SiuneSession.SetProduct(24, Settings.Instance.Key);
            Settings.Instance.Save();

            if (SiuneSession.IsAuthenticated(24))
            {
                Close();
            }
        }
    }
}
