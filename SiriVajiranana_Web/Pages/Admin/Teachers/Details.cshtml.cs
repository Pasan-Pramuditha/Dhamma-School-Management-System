using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Teachers
{
    public class DetailsModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public DetailsModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public Teacher Teacher { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            if (!id.HasValue)
                return RedirectToPage("/Admin/Teachers/Manage");

            string query = "SELECT * FROM Teachers WHERE TeacherID = @TeacherID";
            var dt = await _dbService.ExecuteQueryAsync(query, new[] { new SqlParameter("@TeacherID", id.Value) });

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
                return Page();
            }

            return RedirectToPage("/Admin/Teachers/Manage");
        }
    }
}
