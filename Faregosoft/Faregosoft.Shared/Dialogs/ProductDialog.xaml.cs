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
        public ProductDialog(Product product)
        {
            InitializeComponent();
            Product = product;
            Product.PriceString = $"{Product.Price}";
            Product.InventoryString = $"{Product.Inventory}";
            if (Product.IsEdit)
            {
                TitleTextBlock.Text = $"Editar Producto: {Product.Name}";
            }
            else
            {
                TitleTextBlock.Text = "Nuevo producto";
            }
        }

        public Product Product { get; set; }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = await ValidateFormAsync();
            if (!isValid)
            {
                return;
            }

            Product.WasSaved = true;
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

            decimal.TryParse(Product.PriceString, out decimal price);
            Product.Price = price;
            if (Product.Price < 0)
            {
                messageDialog = new MessageDialog("Debes ingresar un precio al producto superior a cero.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            float.TryParse(Product.InventoryString, out float inventory);
            Product.Inventory = inventory;
            if (Product.Inventory <= 0)
            {
                messageDialog = new MessageDialog("Debes ingresar un inventario al producto positivo.", "Error");
                await messageDialog.ShowAsync();
                return false;
            }

            return true;
        }

        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Hide();
        }

    }
}
