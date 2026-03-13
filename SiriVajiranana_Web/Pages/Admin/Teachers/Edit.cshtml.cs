using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Teachers
{
    public class EditModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public EditModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public Teacher Teacher { get; set; } = new();

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            if (!id.HasValue)
                return RedirectToPage("/Admin/Teachers/Manage");

            await LoadTeacher(id.Value);

            if (string.IsNullOrEmpty(Teacher.Name))
            {
                return RedirectToPage("/Admin/Teachers/Manage");
            }

            return Page();
        }

        private async Task LoadTeacher(int id)
        {
            string query = "SELECT * FROM Teachers WHERE TeacherID = @TeacherID";
            var dt = await _dbService.ExecuteQueryAsync(query, new[] { new SqlParameter("@TeacherID", id) });

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                Teacher = new Teacher
                {
                    TeacherID = (int)row["TeacherID"],
                    Name = row["Name"]?.ToString() ?? "",
                    Gender = row["Gender"]?.ToString() ?? "",
                    NIC = row["NIC"]?.ToString() ?? "",
                    Telephone_No = row["Telephone_No"]?.ToString() ?? "",
                    Address = row["Address"]?.ToString() ?? "",
                    Email = row["Email"]?.ToString() ?? "",
                    Year_of_Appointment = row["Year_of_Appointment"]?.ToString() ?? "",
                    Subject = row["Subject"]?.ToString() ?? ""
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

            string query = @"UPDATE Teachers 
                SET Name = @Name, Gender = @Gender, NIC = @NIC, Telephone_No = @Telephone_No, 
                    Address = @Address, Email = @Email, Year_of_Appointment = @Year_of_Appointment, 
                    Subject = @Subject 
                WHERE TeacherID = @TeacherID";

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
                Message = "Teacher updated successfully!";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating teacher: " + ex.Message;
                return Page();
            }
        }
    }
}
