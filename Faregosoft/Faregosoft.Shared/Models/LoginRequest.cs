﻿namespace Faregosoft.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
