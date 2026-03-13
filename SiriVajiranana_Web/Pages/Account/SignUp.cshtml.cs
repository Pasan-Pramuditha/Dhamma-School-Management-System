using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.ComponentModel.DataAnnotations;

namespace SiriVajiranana_Web.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public SignUpModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [BindProperty]
        public SignUpViewModel Input { get; set; } = new();

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Check if this is the very first registration in the system
                string countQuery = "SELECT COUNT(*) FROM Login";
                var countResult = await _dbService.ExecuteScalarAsync(countQuery);
                int userCount = countResult != DBNull.Value && countResult != null ? Convert.ToInt32(countResult) : 0;

                // Dynamically assign the next Admin ID
                string maxIdQuery = "SELECT ISNULL(MAX(AdminID), 0) + 1 FROM Login";
                var maxIdResult = await _dbService.ExecuteScalarAsync(maxIdQuery);
                string newAdminID = maxIdResult?.ToString() ?? "1";

                string query;
                SqlParameter[] parameters;
                bool isSuperAdmin = false;

                if (userCount == 0)
                {
                    // First user becomes Super Admin instantly
                    isSuperAdmin = true;
                    // Force AdminID to "1" and IsApproved to 1
                    query = "INSERT INTO Login (AdminID, Admin_Name, UserName, Password, IsApproved) VALUES (@AdminID, @Admin_Name, @UserName, @Password, 1)";
                    parameters = new[]
                    {
                        new SqlParameter("@AdminID", newAdminID),
                        new SqlParameter("@Admin_Name", Input.AdminName),
                        new SqlParameter("@UserName", Input.Username),
                        new SqlParameter("@Password", Input.Password)
                    };
                }
                else
                {
                    // Normal user, requires approval
                    query = "INSERT INTO Login (AdminID, Admin_Name, UserName, Password, IsApproved) VALUES (@AdminID, @Admin_Name, @UserName, @Password, 0)";
                    parameters = new[]
                    {
                        new SqlParameter("@AdminID", newAdminID),
                        new SqlParameter("@Admin_Name", Input.AdminName),
                        new SqlParameter("@UserName", Input.Username),
                        new SqlParameter("@Password", Input.Password)
                    };
                }

                int rows = await _dbService.ExecuteNonQueryAsync(query, parameters);
                if (rows > 0)
                {
                    if (isSuperAdmin) 
                    {
                        return RedirectToPage("/Account/Login", new { success = "System Initialized! You are automatically registered as Super Admin. You may log in immediately." });
                    }
                    else
                    {
                        return RedirectToPage("/Account/Login", new { success = "Registration successful! Your account is pending approval from Super Admin." });
                    }
                }
                else
                {
                    ErrorMessage = "Failed to register. Please check your inputs.";
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
            }

            return Page();
        }
    }
}
