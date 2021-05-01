using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Faregosoft.Api2.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public float Inventory { get; set; }

        public bool IsActive { get; set; }

        public User User { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ProductImages == null || ProductImages.Count == 0
            ? $"https://faregosoftapi.azurewebsites.net/images/noimage.png"
            : ProductImages.FirstOrDefault().ImageFullPath;

        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
