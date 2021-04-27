using Faregosoft.Components;
using Faregosoft.Helpers;
using Faregosoft.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Faregosoft.Pages
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.Current;
            Frame rootFrame = window.Content as Frame;
            window.Content = rootFrame;
            rootFrame.Navigate(typeof(RegisterPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidateFormAsync();
            if (!isValid)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.LoginAsync(Settings.GetApiUrl(), "api", "Account", EmailTextBox.Text, PasswordPasswordBox.Password);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Frame.Navigate(typeof(MainPage), token);
        }

        private async Task<bool> ValidateFormAsync()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar tu email.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBox.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(PasswordPasswordBox.Password))
            {
                messageDialog = new MessageDialog("Debes ingresar tu contraseña.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }
    }
}
