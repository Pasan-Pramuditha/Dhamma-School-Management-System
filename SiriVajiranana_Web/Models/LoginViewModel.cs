using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;
    }
}
