using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Students
{
    public class ViewResultsModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public ViewResultsModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public DataTable Results { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            string query = "SELECT * FROM Student_Registration";
            SqlParameter[]? parameters = null;

            if (!string.IsNullOrEmpty(SearchQuery))
            {
                bool isNumeric = int.TryParse(SearchQuery, out int registrationNo);
                if (isNumeric)
                {
                    query += " WHERE Registration_No = @SearchValue";
                    parameters = new[] { new SqlParameter("@SearchValue", registrationNo) };
                }
                else
                {
                    query += " WHERE Name LIKE @SearchValue";
                    parameters = new[] { new SqlParameter("@SearchValue", "%" + SearchQuery + "%") };
                }
            }

            Results = await _dbService.ExecuteQueryAsync(query, parameters);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToPage("/Account/Login");

            string query = "DELETE FROM Student_Registration WHERE Registration_No = @Registration_No";
            await _dbService.ExecuteNonQueryAsync(query, new[] { new SqlParameter("@Registration_No", id) });

            return RedirectToPage();
        }
    }
}
