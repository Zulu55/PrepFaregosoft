using Faregosoft.Models;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Faregosoft.Dialogs
{
    public sealed partial class ProductDialog : ContentDialog
    {
        private Product _product;

        public ProductDialog(Product product)
        {
            InitializeComponent();
            DataContext = this;
            _product = product;
            _product.PriceString = $"{_product.Price}";
            _product.InventoryString = $"{_product.Inventory}";
            if (_product.IsEdit)
            {
                TitleTextBlock.Text = $"Editar Producto: {_product.Name}";
            }
            else
            {
                TitleTextBlock.Text = "Nuevo prodcuto";
            }
        }

        public Product Product
        {
            get => _product;
            set => _product = value;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidateFormAsync();
            if (!isValid)
            {
                return;
            }

            _product.WasSaved = true;
            Hide();
        }

        private async Task<bool> ValidateFormAsync()
        {
            MessageDialog messageDialog;

            if (string.IsNullOrEmpty(Product.Name))
            {
                messageDialog = new MessageDialog("Debes ingresar un nombre al prodcuto.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            if (string.IsNullOrEmpty(Product.Description))
            {
                messageDialog = new MessageDialog("Debes ingresar una descripción al prodcuto.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            decimal price;
            decimal.TryParse(Product.PriceString, out price);
            Product.Price = price;
            if (Product.Price < 0)
            {
                messageDialog = new MessageDialog("Debes ingresar un precio al producto superior a cero.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            float inventory;
            float.TryParse(Product.InventoryString, out inventory);
            Product.Inventory = inventory;
            if (Product.Inventory <= 0)
            {
                messageDialog = new MessageDialog("Debes ingresar un inventario al prodcuto positivo.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _product.WasSaved = false;
            Hide();
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _product.WasSaved = false;
            Hide();
        }
    }
}
