using System;

namespace Faregosoft.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PriceString { get; set; }

        public string PriceFormated => $"{Price:C2}";

        public float Inventory { get; set; }

        public string InventoryString { get; set; }

        public string InventoryFormated => $"{Inventory:N2}";

        public bool IsActive { get; set; }

        public Guid Guid { get; set; }

        public User User { get; set; }

        public bool WasSaved { get; set; }

        public bool IsEdit { get; set; }
    }
}
