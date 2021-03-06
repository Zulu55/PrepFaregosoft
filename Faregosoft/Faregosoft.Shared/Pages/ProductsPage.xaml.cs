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
        private int _totalRecords;
        private Pagination _pagination;
        private Loader _loader;

        public ProductsPage()
        {
            InitializeComponent();
            LoadProductsAsync();
        }

        public ObservableCollection<Product> Products { get; set; }

        private async Task LoadProductsAsync()
        {
            _loader = new Loader("Por favor espere...");
            _loader.Show();
            Response responseCount = await ApiService.GetCountAsync(Settings.GetApiUrl(), "api", "Products", MainPage.GetInstance().Token.Token);
            _loader.Close();
            if (!responseCount.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(responseCount.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            _totalRecords = (int)responseCount.Result;
            _pagination = new Pagination(_totalRecords);
            _pagination.PageChanged += Pagination_PageChanged;
            MyPagination.Children.Clear();
            MyPagination.Children.Add(_pagination);

            await GetProductsAsync();
        }

        private async Task GetProductsAsync()
        {
            _loader.Show();
            Response responseList = await ApiService.GetListPagedAsync<Product>(Settings.GetApiUrl(), "api", "Products", _pagination.Page - 1, _pagination.Size, MainPage.GetInstance().Token.Token);
            _loader.Close();

            if (!responseList.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(responseList.Message, "Error");
                await dialog.ShowAsync();
                return;
            }

            List<Product> products = (List<Product>)responseList.Result;
            Products = new ObservableCollection<Product>(products);
            RefreshList();
        }

        private async void Pagination_PageChanged(object sender, EventArgs e)
        {
            _loader.Show();
            await GetProductsAsync();
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

            product.User = MainPage.GetInstance().Token.User;
            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PostAsync(Settings.GetApiUrl(), "api", "Products", product, MainPage.GetInstance().Token.Token);
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

            Loader loader = new Loader("Por favor espere...");
            loader.Show();
            Response response = await ApiService.PutAsync(Settings.GetApiUrl(), "api", "Products", product, product.Id, MainPage.GetInstance().Token.Token);
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
            Response response = await ApiService.DeleteAsync<Product>(Settings.GetApiUrl(), "api", "Products", product.Id, MainPage.GetInstance().Token.Token);
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
