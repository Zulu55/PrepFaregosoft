using Faregosoft.Helpers;
using Faregosoft.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Faregosoft.Pages
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidFormAsync();
            if (!isValid)
            {
                return;
            }

            RegisterUserModel model = new RegisterUserModel
            {
                FirstName = FirstNameTextBlock.Text,
                LastName = LastNameTextBlock.Text,
                Email = EmailTextBlock.Text,
                Password = PasswordPasswordBox.Password
            };

            MessageDialog messageDialog;
            Response response = await ApiService.RegisterAsync("https://faregosoftapi.azurewebsites.net/", "api", "Users", model);
            if (!response.IsSuccess)
            {
                messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            messageDialog = new MessageDialog($"Usuario: {model.FirstName} {model.LastName}, registrado.", "Ok");
            await messageDialog.ShowAsync();
            Frame.Navigate(typeof(LoginPage));
        }

        private async Task<bool> ValidFormAsync()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(FirstNameTextBlock.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar los nombres del usuario.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(LastNameTextBlock.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar los apellidos del usuario.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(EmailTextBlock.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (!RegexUtilities.IsValidEmail(EmailTextBlock.Text))
            {
                messageDialog = new MessageDialog("Debes ingresar un email válido.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (PasswordPasswordBox.Password.Length < 6)
            {
                messageDialog = new MessageDialog("Debes ingresar una contraseña de al menos 6 carácteres.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (PasswordConfirmPasswordBox.Password.Length < 6)
            {
                messageDialog = new MessageDialog("Debes ingresar una confirmación contraseña de al menos 6 carácteres.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (PasswordPasswordBox.Password != PasswordConfirmPasswordBox.Password)
            {
                messageDialog = new MessageDialog("La confirmación y la contraseña no son iguales.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }
    }
}
