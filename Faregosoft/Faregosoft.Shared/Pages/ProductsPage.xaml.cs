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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Faregosoft.Pages
{
    public sealed partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProductsAsync();
        }

        public ObservableCollection<Product> Products { get; set; }

        private async Task LoadProductsAsync()
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.GetListAsync<Product>("https://localhost:44377/", "api", "Products");
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Product> products = (List<Product>)response.Result;
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private void RefreshList()
        {
            ProductsListView.ItemsSource = null;
            ProductsListView.Items.Clear();
            ProductsListView.ItemsSource = Products;
        }

        private async void AddProductButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Product product = new Product();
            ProductDialog dialog = new ProductDialog(product);
            await dialog.ShowAsync();
            if (!product.WasSaved)
            {
                return;
            }

            product.User = MainPage.GetInstance().User;
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync("https://localhost:44377/", "api", "Products", product);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Product newProduct = (Product)response.Result;
            Products.Add(newProduct);
            Products = new ObservableCollection<Product>(Products.OrderBy(p => p.Name).ToList());
            RefreshList();
        }

        private async void EditImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Product product = Products[ProductsListView.SelectedIndex];
            product.IsEdit = true;
            ProductDialog dialog = new ProductDialog(product);
            await dialog.ShowAsync();

            if (!product.WasSaved)
            {
                return;
            }

            product.User = MainPage.GetInstance().User;
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PutAsync("https://localhost:44377/", "api", "Products", product, product.Id);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            Product newProduct = (Product)response.Result;
            Product oldProduct = Products.FirstOrDefault(p => p.Id == newProduct.Id);
            oldProduct = newProduct;
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
            Product product = Products[ProductsListView.SelectedIndex];
            Response response = await ApiService.DeleteAsync<Product>("https://localhost:44377/", "api", "Products", product.Id);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog messageDialog = new MessageDialog(response.Message, "Error");
                await messageDialog.ShowAsync();
                return;
            }

            List<Product> products = Products.Where(p => p.Id != product.Id).ToList();
            Products = new ObservableCollection<Product>(products);
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
