using System.ComponentModel.DataAnnotations;

namespace Faregosoft.Api2.Models
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
