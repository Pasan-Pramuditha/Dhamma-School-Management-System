using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Admin_Name { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;
    }
}
