using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Models
{
    public class Teacher
    {
        [Key]
        [Required(ErrorMessage = "Teacher ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Teacher ID must be greater than 0")]
        public int TeacherID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(20)]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "NIC is required")]
        [StringLength(20)]
        public string NIC { get; set; } = "";

        [Required(ErrorMessage = "Telephone Number is required")]
        [StringLength(20)]
        public string Telephone_No { get; set; } = "";

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; } = "";

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Year of Appointment is required")]
        [StringLength(20)]
        public string Year_of_Appointment { get; set; } = "";

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100)]
        public string Subject { get; set; } = "";
    }
}
