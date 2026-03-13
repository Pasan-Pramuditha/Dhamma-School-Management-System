using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;
using SiriVajiranana_Web.Models;
using System.Data;

namespace SiriVajiranana_Web.Pages.Admin.Students
{
    public class DetailsModel : PageModel
    {
        private readonly DatabaseService _dbService;

        public DetailsModel(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public Student Student { get; set; } = new();
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
            try
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
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
