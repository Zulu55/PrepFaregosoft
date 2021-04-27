using Faregosoft.Components;
using Faregosoft.Dialogs;
using Faregosoft.Helpers;
using Faregosoft.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Faregosoft.Pages
{
    public sealed partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
            LoadCustomersAsync();
        }

        public ObservableCollection<Customer> Customers { get; set; }

        private async Task LoadCustomersAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.GetListAsync<Customer>(Settings.GetApiUrl(), "api", "Customers", MainPage.GetInstance().Token.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Customer> customers = (List<Customer>)response.Result;
            Customers = new ObservableCollection<Customer>(customers);
            RefreshList();
        }

        private void RefreshList()
        {
            CustomersListView.ItemsSource = null;
            CustomersListView.Items.Clear();
            CustomersListView.ItemsSource = Customers;
        }

        private async void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer();
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();
            if (!customer.WasSaved)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync(Settings.GetApiUrl(), "api", "Customers", customer, MainPage.GetInstance().Token.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customers.Add(newCustomer);
            Customers = new ObservableCollection<Customer>(Customers.OrderBy(c => c.FirstName).ToList());
            RefreshList();
        }

        private async void EditImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Customer customer = Customers[CustomersListView.SelectedIndex];
            customer.IsEdit = true;
            CustomerDialog dialog = new CustomerDialog(customer);
            await dialog.ShowAsync();

            if (!customer.WasSaved)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PutAsync(Settings.GetApiUrl(), "api", "Customers", customer, customer.Id, MainPage.GetInstance().Token.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Customer newCustomer = (Customer)response.Result;
            Customer oldCustomer = Customers.FirstOrDefault(c => c.Id == newCustomer.Id);
            oldCustomer = newCustomer;
            RefreshList();
        }

        private async void DeleteImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialogResult result = await ConfirmDeleteAsync();
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Customer customer = Customers[CustomersListView.SelectedIndex];
            Response response = await ApiService.DeleteAsync<Customer>(Settings.GetApiUrl(), "api", "Customers", customer.Id, MainPage.GetInstance().Token.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            List<Customer> customers = Customers.Where(c => c.Id != customer.Id).ToList();
            Customers = new ObservableCollection<Customer>(customers);
            RefreshList();
        }

        private async Task<ContentDialogResult> ConfirmDeleteAsync()
        {
            ContentDialog confirmDialog = new ContentDialog
            {
                Title = "Confirmación",
                Content = "¿Estas seguro de querer borrar el registro?",
                PrimaryButtonText = "Si",
                CloseButtonText = "No"
            };

            return await confirmDialog.ShowAsync();
        }
    }
}
