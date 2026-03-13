using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Models
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "Registration Number is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Registration Number must be greater than 0")]
        public int Registration_No { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(255)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(20)]
        public string Gender { get; set; } = "";

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255)]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "Telephone Number is required")]
        [StringLength(20)]
        public string Telephone_No { get; set; } = "";

        public string? Other_Skills { get; set; }
        
        [Required(ErrorMessage = "Year of Admission is required")]
        [StringLength(50)]
        public string Year_of_Admission { get; set; } = "";
        
        // Grades setup
        public string? Grade05_Buddhism { get; set; }
        public string? Grade05_Abhidhamma { get; set; }
        public string? Grade05_Pali { get; set; }
        public string? Grade06_Buddhism { get; set; }
        public string? Grade06_Abhidhamma { get; set; }
        public string? Grade06_Pali { get; set; }
        public string? Grade07_Buddhism { get; set; }
        public string? Grade07_Abhidhamma { get; set; }
        public string? Grade07_Pali { get; set; }
        public string? Grade08_Buddhism { get; set; }
        public string? Grade08_Abhidhamma { get; set; }
        public string? Grade08_Pali { get; set; }
        public string? Grade09_Buddhism { get; set; }
        public string? Grade09_Abhidhamma { get; set; }
        public string? Grade09_Pali { get; set; }
        public string? Grade10_Buddhism { get; set; }
        public string? Grade10_Abhidhamma { get; set; }
        public string? Grade10_Pali { get; set; }
        public string? Grade11_Buddhism { get; set; }
        public string? Grade11_Abhidhamma { get; set; }
        public string? Grade11_Pali { get; set; }
    }
}
