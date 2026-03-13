using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;

namespace SiriVajiranana_Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public LoginModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Clear session on load if already logged in (optional)
            HttpContext.Session.Clear();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string query = "SELECT IsApproved, AdminID FROM Login WHERE UserName = @username AND Password = @password";
            SqlParameter[] parameters = new[]
            {
                new SqlParameter("@username", Input.Username),
                new SqlParameter("@password", Input.Password)
            };

            var dt = await _dbService.ExecuteQueryAsync(query, parameters);

            if (dt.Rows.Count == 0)
            {
                ErrorMessage = "Login failed. Please check your username and password.";
                return Page();
            }

            var row = dt.Rows[0];
            bool isApproved = Convert.ToBoolean(row["IsApproved"]);
            string adminId = row["AdminID"].ToString() ?? "";

            if (!isApproved)
            {
                ErrorMessage = "Your account is pending approval from Super Admin. You cannot log in yet.";
                return Page();
            }

            HttpContext.Session.SetString("User", Input.Username);
            HttpContext.Session.SetString("AdminID", adminId);
            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
