using System;

namespace Faregosoft.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Guid Image { get; set; }

        public string ImageFullPath => Image == Guid.Empty
            ? $"https://faregosoftapi.azurewebsites.net/images/noimage.png"
            : $"https://faregosoft.blob.core.windows.net/products/{Image}";
    }
}
