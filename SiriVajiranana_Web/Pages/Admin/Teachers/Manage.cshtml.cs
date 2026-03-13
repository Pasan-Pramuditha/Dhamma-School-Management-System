using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Teachers
{
    public class ManageModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public ManageModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public List<Teacher> Teachers { get; set; } = new();
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            await LoadTeachers();
            return Page();
        }

        private async Task LoadTeachers()
        {
            string query = "SELECT * FROM Teachers";
            SqlParameter[]? parameters = null;

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query += " WHERE Name LIKE @search OR CAST(TeacherID as NVARCHAR) LIKE @search";
                parameters = new[] { new SqlParameter("@search", "%" + SearchQuery + "%") };
            }

            query += " ORDER BY Name";
            var dt = await _dbService.ExecuteQueryAsync(query, parameters);
            
            Teachers = dt.AsEnumerable().Select(row => new Teacher
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
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            string query = "DELETE FROM Teachers WHERE TeacherID = @TeacherID";
            try
            {
                await _dbService.ExecuteNonQueryAsync(query, new[] { new SqlParameter("@TeacherID", id) });
                Message = "Teacher deleted successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error deleting teacher: " + ex.Message;
            }

            await LoadTeachers();
            return Page();
        }
    }
}
