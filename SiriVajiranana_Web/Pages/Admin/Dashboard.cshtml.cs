using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SiriVajiranana_Web.Data;

namespace SiriVajiranana_Web.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public DashboardModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        public string? Username { get; set; }
        public string? AdminID { get; set; }
        public string? AdminName { get; set; }
        public List<PendingAdmin> PendingAdmins { get; set; } = new();

        public class PendingAdmin
        {
            public string AdminID { get; set; } = string.Empty;
            public string AdminName { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string RegistrationDate { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Username = HttpContext.Session.GetString("User");
            AdminID = HttpContext.Session.GetString("AdminID");

            if (string.IsNullOrEmpty(Username))
            {
                return RedirectToPage("/Account/Login");
            }

            // Fetch the logged-in admin's actual name
            string nameQuery = "SELECT Admin_Name FROM Login WHERE AdminID = @id";
            var nameResult = await _dbService.ExecuteScalarAsync(nameQuery, new[] { new Microsoft.Data.SqlClient.SqlParameter("@id", AdminID ?? "") });
            AdminName = nameResult?.ToString() ?? Username;

            // Get total students
            string studentQuery = "SELECT COUNT(*) FROM Student_Registration";
            var studentResult = await _dbService.ExecuteScalarAsync(studentQuery);
            StudentCount = (int)(studentResult ?? 0);

            // Get total teachers
            string teacherQuery = "SELECT COUNT(*) FROM Teachers";
            var teacherResult = await _dbService.ExecuteScalarAsync(teacherQuery);
            TeacherCount = (int)(teacherResult ?? 0);

            // Fetch pending admins if super admin (1 or 01)
            if (AdminID == "01" || AdminID == "1")
            {
                string pendingQuery = "SELECT AdminID, Admin_Name, UserName FROM Login WHERE IsApproved = 0";
                var dt = await _dbService.ExecuteQueryAsync(pendingQuery);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    PendingAdmins.Add(new PendingAdmin
                    {
                        AdminID = row["AdminID"]?.ToString() ?? "",
                        AdminName = row["Admin_Name"]?.ToString() ?? "",
                        Username = row["UserName"]?.ToString() ?? ""
                    });
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(string id)
        {
            var sessionAdmin = HttpContext.Session.GetString("AdminID");
            if (sessionAdmin != "01" && sessionAdmin != "1") return Unauthorized();

            string query = "UPDATE Login SET IsApproved = 1 WHERE AdminID = @id";
            await _dbService.ExecuteNonQueryAsync(query, new[] { new Microsoft.Data.SqlClient.SqlParameter("@id", id) });
            
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(string id)
        {
            var sessionAdmin = HttpContext.Session.GetString("AdminID");
            if (sessionAdmin != "01" && sessionAdmin != "1") return Unauthorized();

            string query = "DELETE FROM Login WHERE AdminID = @id";
            await _dbService.ExecuteNonQueryAsync(query, new[] { new Microsoft.Data.SqlClient.SqlParameter("@id", id) });
            
            return RedirectToPage();
        }
    }
}
