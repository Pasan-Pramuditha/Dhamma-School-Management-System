using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Admin Name is required")]
        [StringLength(255)]
        public string AdminName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
