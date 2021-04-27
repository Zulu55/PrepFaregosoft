using Faregosoft.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Faregosoft.Pages
{
    public sealed partial class MainPage : Page
    {
        private static MainPage _instance;

        public MainPage()
        {
            InitializeComponent();
            _instance = this;
        }

        public TokenResponse Token { get; set; }

        public static MainPage GetInstance()
        {
            return _instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Token = (TokenResponse)e.Parameter;
            WelcomeTextBlock.Text = $"Bienvenido: {Token.User.FirstName} {Token.User.LastName}!";
            MyFrame.Navigate(typeof(CustomersPage));
        }

        private void CustomersNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(CustomersPage));
        }

        private async Task<ContentDialogResult> ConfirmLeaveAsync()
        {
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Confirmación",
                Content = "Estas seguro de salir?",
                PrimaryButtonText = "Sí",
                CloseButtonText = "No"
            };

            return await confirmDialog.ShowAsync();
        }

        private async void LogoutImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialogResult dialog = await ConfirmLeaveAsync();
            if (dialog == ContentDialogResult.Primary)
            {
                Frame.Navigate(typeof(LoginPage));
            }
        }

        private void ProductosNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(ProductsPage));
        }
    }
}
