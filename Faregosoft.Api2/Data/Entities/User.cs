using Faregosoft.Api2.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Faregosoft.Api2.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public ICollection<Customer> Customers { get; set; }
    }
}
