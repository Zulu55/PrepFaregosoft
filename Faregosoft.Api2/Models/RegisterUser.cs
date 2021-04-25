using System.ComponentModel.DataAnnotations;

namespace Faregosoft.Api2.Models
{
    public class RegisterUser
    {
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
    }
}
