using PrepFaregosoft.Helpers;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PrepFaregosoft.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageDialog messageDialog = new MessageDialog("Debes ingresar un email.", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                MessageDialog messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(PasswordPasswordBox.Password))
            {
                MessageDialog messageDialog = new MessageDialog("Debes ingresar una contraseña.", "Error");
                await messageDialog.ShowAsync();
                return;
            }
        }
    }
}
