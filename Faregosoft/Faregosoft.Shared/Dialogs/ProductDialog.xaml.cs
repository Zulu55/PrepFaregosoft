using Faregosoft.Models;
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

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            _product.WasSaved = true;
            Hide();
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
