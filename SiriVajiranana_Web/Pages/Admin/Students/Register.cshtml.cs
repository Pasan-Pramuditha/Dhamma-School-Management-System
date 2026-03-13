using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Students
{
    public class RegisterModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public RegisterModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Student Student { get; set; } = new();

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
            string query = @"INSERT INTO Student_Registration 
                (Registration_No, Name, Gender, Address, Telephone_No, Other_Skills, Year_of_Admission, 
                Grade05_Buddhism, Grade05_Abhidhamma, Grade05_Pali, Grade06_Buddhism, Grade06_Abhidhamma, Grade06_Pali, 
                Grade07_Buddhism, Grade07_Abhidhamma, Grade07_Pali, Grade08_Buddhism, Grade08_Abhidhamma, Grade08_Pali, 
                Grade09_Buddhism, Grade09_Abhidhamma, Grade09_Pali, Grade10_Buddhism, Grade10_Abhidhamma, Grade10_Pali, 
                Grade11_Buddhism, Grade11_Abhidhamma, Grade11_Pali) 
                VALUES 
                (@Registration_No, @Name, @Gender, @Address, @Telephone_No, @Other_Skills, @Year_of_Admission, 
                @Grade05_Buddhism, @Grade05_Abhidhamma, @Grade05_Pali, @Grade06_Buddhism, @Grade06_Abhidhamma, @Grade06_Pali, 
                @Grade07_Buddhism, @Grade07_Abhidhamma, @Grade07_Pali, @Grade08_Buddhism, @Grade08_Abhidhamma, @Grade08_Pali, 
                @Grade09_Buddhism, @Grade09_Abhidhamma, @Grade09_Pali, @Grade10_Buddhism, @Grade10_Abhidhamma, @Grade10_Pali, 
                @Grade11_Buddhism, @Grade11_Abhidhamma, @Grade11_Pali)";

            try
            {
                await _dbService.ExecuteNonQueryAsync(query, GetParameters());
                Message = "Student Registered Successfully!";
                ModelState.Clear();
                Student = new(); // Clear
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
            }
            return Page();
        }

        private SqlParameter[] GetParameters()
        {
            return new[]
            {
                new SqlParameter("@Registration_No", Student.Registration_No),
                new SqlParameter("@Name", Student.Name ?? ""),
                new SqlParameter("@Gender", Student.Gender ?? ""),
                new SqlParameter("@Address", Student.Address ?? ""),
                new SqlParameter("@Telephone_No", Student.Telephone_No ?? ""),
                new SqlParameter("@Other_Skills", Student.Other_Skills ?? ""),
                new SqlParameter("@Year_of_Admission", Student.Year_of_Admission ?? ""),
                new SqlParameter("@Grade05_Buddhism", Student.Grade05_Buddhism ?? ""),
                new SqlParameter("@Grade05_Abhidhamma", Student.Grade05_Abhidhamma ?? ""),
                new SqlParameter("@Grade05_Pali", Student.Grade05_Pali ?? ""),
                new SqlParameter("@Grade06_Buddhism", Student.Grade06_Buddhism ?? ""),
                new SqlParameter("@Grade06_Abhidhamma", Student.Grade06_Abhidhamma ?? ""),
                new SqlParameter("@Grade06_Pali", Student.Grade06_Pali ?? ""),
                new SqlParameter("@Grade07_Buddhism", Student.Grade07_Buddhism ?? ""),
                new SqlParameter("@Grade07_Abhidhamma", Student.Grade07_Abhidhamma ?? ""),
                new SqlParameter("@Grade07_Pali", Student.Grade07_Pali ?? ""),
                new SqlParameter("@Grade08_Buddhism", Student.Grade08_Buddhism ?? ""),
                new SqlParameter("@Grade08_Abhidhamma", Student.Grade08_Abhidhamma ?? ""),
                new SqlParameter("@Grade08_Pali", Student.Grade08_Pali ?? ""),
                new SqlParameter("@Grade09_Buddhism", Student.Grade09_Buddhism ?? ""),
                new SqlParameter("@Grade09_Abhidhamma", Student.Grade09_Abhidhamma ?? ""),
                new SqlParameter("@Grade09_Pali", Student.Grade09_Pali ?? ""),
                new SqlParameter("@Grade10_Buddhism", Student.Grade10_Buddhism ?? ""),
                new SqlParameter("@Grade10_Abhidhamma", Student.Grade10_Abhidhamma ?? ""),
                new SqlParameter("@Grade10_Pali", Student.Grade10_Pali ?? ""),
                new SqlParameter("@Grade11_Buddhism", Student.Grade11_Buddhism ?? ""),
                new SqlParameter("@Grade11_Abhidhamma", Student.Grade11_Abhidhamma ?? ""),
                new SqlParameter("@Grade11_Pali", Student.Grade11_Pali ?? "")
            };
        }
    }
}
