using Faregosoft.Components;
using Faregosoft.Helpers;
using Faregosoft.Models;
using Faregosoft.Pages;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
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
                DialogGrid.Height = 300;
                ImagesGridView.Visibility = Visibility.Collapsed;
                TakePictureButton.Visibility = Visibility.Collapsed;
                SelectPictureButton.Visibility = Visibility.Collapsed;
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

        private async void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                return;
            }

            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("Products",
                    CreationCollisionOption.OpenIfExists);

            string fileName = $"{Guid.NewGuid()}.jpg";
            await photo.CopyAsync(destinationFolder, fileName, NameCollisionOption.ReplaceExisting);
            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            AddProductImageRequest model = await UploadImageAsync(stream);
            if (model != null)
            {
                Product.ProductImages.Add(new ProductImage
                {
                    Image = model.Guid
                });

                RefreshList();
            }
        }

        private void RefreshList()
        {
            ImagesGridView.ItemsSource = null;
            ImagesGridView.Items.Clear();
            ImagesGridView.ItemsSource = Product.ProductImages;
        }

        private async Task<AddProductImageRequest> UploadImageAsync(IRandomAccessStream stream)
        {
            Loader loader = new Loader("Por favor espere...");
            loader.Show();

            byte[] bytes = await ConverterHelper.ToByteArray(stream);
            AddProductImageRequest model = new AddProductImageRequest
            {
                ProductId = Product.Id,
                Image = bytes
            };

            Response response = await ApiService.PostAsync(Settings.GetApiUrl(), "api", "ProductImages", model, MainPage.GetInstance().Token.Token);
            loader.Close();

            if (!response.IsSuccess)
            {
                MessageDialog dialog = new MessageDialog(response.Message, "Error");
                await dialog.ShowAsync();
                return null; 
            }

            model = (AddProductImageRequest)response.Result;
            return model;
        }

        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker picker = new Windows.Storage.Pickers.FileOpenPicker
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }

            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
            AddProductImageRequest model = await UploadImageAsync(stream);
            if (model != null)
            {
                Product.ProductImages.Add(new ProductImage
                {
                    Image = model.Guid
                });

                RefreshList();
            }
        }
    }
}
