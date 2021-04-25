using System;
using System.ComponentModel.DataAnnotations;

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

        public Guid Guid { get; set; }

        public User User { get; set; }
    }
}
