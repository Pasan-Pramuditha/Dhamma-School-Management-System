using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public EditModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Student Student { get; set; } = new();

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            if (!id.HasValue)
                return RedirectToPage("/Admin/Students/ViewResults");

            await LoadStudent(id.Value);

            if (string.IsNullOrEmpty(Student.Name))
            {
                return RedirectToPage("/Admin/Students/ViewResults");
            }

            return Page();
        }

        private async Task LoadStudent(int regNo)
        {
            string query = "SELECT * FROM Student_Registration WHERE Registration_No = @Registration_No";
            var dt = await _dbService.ExecuteQueryAsync(query, new[] { new SqlParameter("@Registration_No", regNo) });

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                Student = new Student
                {
                    Registration_No = (int)row["Registration_No"],
                    Name = row["Name"]?.ToString() ?? "",
                    Gender = row["Gender"]?.ToString() ?? "",
                    Address = row["Address"]?.ToString() ?? "",
                    Telephone_No = row["Telephone_No"]?.ToString() ?? "",
                    Other_Skills = row["Other_Skills"]?.ToString() ?? "",
                    Year_of_Admission = row["Year_of_Admission"]?.ToString() ?? "",
                    Grade05_Buddhism = row["Grade05_Buddhism"]?.ToString() ?? "",
                    Grade05_Abhidhamma = row["Grade05_Abhidhamma"]?.ToString() ?? "",
                    Grade05_Pali = row["Grade05_Pali"]?.ToString() ?? "",
                    Grade06_Buddhism = row["Grade06_Buddhism"]?.ToString() ?? "",
                    Grade06_Abhidhamma = row["Grade06_Abhidhamma"]?.ToString() ?? "",
                    Grade06_Pali = row["Grade06_Pali"]?.ToString() ?? "",
                    Grade07_Buddhism = row["Grade07_Buddhism"]?.ToString() ?? "",
                    Grade07_Abhidhamma = row["Grade07_Abhidhamma"]?.ToString() ?? "",
                    Grade07_Pali = row["Grade07_Pali"]?.ToString() ?? "",
                    Grade08_Buddhism = row["Grade08_Buddhism"]?.ToString() ?? "",
                    Grade08_Abhidhamma = row["Grade08_Abhidhamma"]?.ToString() ?? "",
                    Grade08_Pali = row["Grade08_Pali"]?.ToString() ?? "",
                    Grade09_Buddhism = row["Grade09_Buddhism"]?.ToString() ?? "",
                    Grade09_Abhidhamma = row["Grade09_Abhidhamma"]?.ToString() ?? "",
                    Grade09_Pali = row["Grade09_Pali"]?.ToString() ?? "",
                    Grade10_Buddhism = row["Grade10_Buddhism"]?.ToString() ?? "",
                    Grade10_Abhidhamma = row["Grade10_Abhidhamma"]?.ToString() ?? "",
                    Grade10_Pali = row["Grade10_Pali"]?.ToString() ?? "",
                    Grade11_Buddhism = row["Grade11_Buddhism"]?.ToString() ?? "",
                    Grade11_Abhidhamma = row["Grade11_Abhidhamma"]?.ToString() ?? "",
                    Grade11_Pali = row["Grade11_Pali"]?.ToString() ?? ""
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields correctly.";
                return Page();
            }

            string query = @"UPDATE Student_Registration 
                SET Name = @Name, Gender = @Gender, Address = @Address, Telephone_No = @Telephone_No, Other_Skills = @Other_Skills, 
                    Year_of_Admission = @Year_of_Admission, 
                    Grade05_Buddhism = @Grade05_Buddhism, Grade05_Abhidhamma = @Grade05_Abhidhamma, Grade05_Pali = @Grade05_Pali, 
                    Grade06_Buddhism = @Grade06_Buddhism, Grade06_Abhidhamma = @Grade06_Abhidhamma, Grade06_Pali = @Grade06_Pali, 
                    Grade07_Buddhism = @Grade07_Buddhism, Grade07_Abhidhamma = @Grade07_Abhidhamma, Grade07_Pali = @Grade07_Pali, 
                    Grade08_Buddhism = @Grade08_Buddhism, Grade08_Abhidhamma = @Grade08_Abhidhamma, Grade08_Pali = @Grade08_Pali, 
                    Grade09_Buddhism = @Grade09_Buddhism, Grade09_Abhidhamma = @Grade09_Abhidhamma, Grade09_Pali = @Grade09_Pali, 
                    Grade10_Buddhism = @Grade10_Buddhism, Grade10_Abhidhamma = @Grade10_Abhidhamma, Grade10_Pali = @Grade10_Pali, 
                    Grade11_Buddhism = @Grade11_Buddhism, Grade11_Abhidhamma = @Grade11_Abhidhamma, Grade11_Pali = @Grade11_Pali 
                WHERE Registration_No = @Registration_No";

            try
            {
                await _dbService.ExecuteNonQueryAsync(query, GetParameters());
                Message = "Student Updated Successfully!";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating record: " + ex.Message;
                return Page();
            }
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
