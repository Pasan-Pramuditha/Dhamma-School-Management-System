using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;

namespace SiriVajiranana_Web.Pages.Admin.Teachers
{
    public class RegisterModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public RegisterModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = new();

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields correctly.";
                return Page();
            }

            string query = @"INSERT INTO Teachers 
                (TeacherID, Name, Gender, NIC, Telephone_No, Address, Email, Year_of_Appointment, Subject) 
                VALUES 
                (@TeacherID, @Name, @Gender, @NIC, @Telephone_No, @Address, @Email, @Year_of_Appointment, @Subject)";

            try
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@TeacherID", Teacher.TeacherID),
                    new SqlParameter("@Name", Teacher.Name),
                    new SqlParameter("@Gender", Teacher.Gender),
                    new SqlParameter("@NIC", Teacher.NIC),
                    new SqlParameter("@Telephone_No", Teacher.Telephone_No),
                    new SqlParameter("@Address", Teacher.Address),
                    new SqlParameter("@Email", (object)Teacher.Email ?? DBNull.Value),
                    new SqlParameter("@Year_of_Appointment", Teacher.Year_of_Appointment),
                    new SqlParameter("@Subject", Teacher.Subject)
                };

                await _dbService.ExecuteNonQueryAsync(query, parameters);
                Message = "Teacher Registered Successfully!";
                ModelState.Clear();
                Teacher = new();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
            return Page();
        }
    }
}
