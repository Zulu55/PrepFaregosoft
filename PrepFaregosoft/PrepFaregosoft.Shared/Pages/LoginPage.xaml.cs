using PrepFaregosoft.Helpers;
using PrepFaregosoft.Models;
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
            MessageDialog messageDialog;
            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email.", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(PasswordPasswordBox.Password))
            {
                messageDialog = new MessageDialog("Debes ingresar una contraseña.", "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Response response = await ApiService.LoginAsync("https://localhost:44347/", "api", "Users", EmailTextBox.Text, PasswordPasswordBox.Password);
            if (!response.IsSuccess)
            {
                messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            User user = (User)response.Result;

            messageDialog = new MessageDialog($"Bienvenido: {user.FirstName} {user.LastName}", "Ok");
            await messageDialog.ShowAsync();
            Frame.Navigate(typeof(MainPage));
        }
    }
}
