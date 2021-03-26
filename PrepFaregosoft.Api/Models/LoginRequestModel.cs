using System.ComponentModel.DataAnnotations;

namespace PrepFaregosoft.Api.Models
{
    public class LoginRequestModel
    {
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

