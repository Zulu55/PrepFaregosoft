using System;

namespace Faregosoft.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }

        public User User { get; set; }
    }
}
