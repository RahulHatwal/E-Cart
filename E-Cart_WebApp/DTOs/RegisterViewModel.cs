using System.ComponentModel.DataAnnotations;
namespace E_Cart_WebApp.DTOs
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]    
        [Compare("Password", ErrorMessage ="Password and Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}