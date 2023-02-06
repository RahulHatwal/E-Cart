using System.ComponentModel.DataAnnotations;

namespace E_Cart_WebApp.DTOs
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set;}
    }
}
