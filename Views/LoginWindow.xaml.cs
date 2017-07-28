using System.Windows;
using System.Windows.Input;
using Arcade.AuthCore;
using Arcade.Models;

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

            Settings.Instance.Key = TxtKey.Text;
            Settings.Instance.Email = TxtEmail.Text;
            AuthCoreSession.SetProduct(2, Settings.Instance.Key);
            Settings.Instance.Save();
        }

        private void TestKey(object sender, RoutedEventArgs e)
        {
            TxtAuth.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(TxtKey.Text) || string.IsNullOrEmpty(TxtEmail.Text))
            {
                TxtAuth.Text = Languages.Language.Instance.LoginMoreInformationNeeded;
                return;
            }

            Settings.Instance.Key = TxtKey.Text;
            Settings.Instance.Email = TxtEmail.Text;
            AuthCoreSession.SetProduct(2, Settings.Instance.Key);
            Settings.Instance.Save();

            if (AuthCoreSession.IsAuthenticated(2))
            {
                Close();
            }
        }
    }
}
