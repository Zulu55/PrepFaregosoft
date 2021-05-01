using System;

namespace Faregosoft.Models
{
    public class AddProductImageRequest
    {
        public int ProductId { get; set; }

        public byte[] Image { get; set; }

        public Guid Guid { get; set; }
    }
}
