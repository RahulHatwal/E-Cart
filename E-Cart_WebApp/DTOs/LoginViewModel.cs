using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Cart_WebApp.DTOs
{

    public partial class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
