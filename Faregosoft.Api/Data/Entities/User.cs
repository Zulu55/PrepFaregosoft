using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Faregosoft.Api.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(4)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public bool IsBlock { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
