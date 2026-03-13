using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SiriVajiranana_Web.Data;

namespace SiriVajiranana_Web.Pages.Admin
{
    public class BackupModel : PageModel
    {
        private readonly DatabaseService _dbService;
        private readonly IWebHostEnvironment _env;

        public BackupModel(DatabaseService dbService, IWebHostEnvironment env)
        {
            _dbService = dbService;
            _env = env;
        }

        [BindProperty]
        public string? BackupPath { get; set; }

        [BindProperty]
        public IFormFile? RestoreFile { get; set; }

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            BackupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Backups");
        }

        public IActionResult OnGetDrives()
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(d => new { name = d.Name, path = d.Name })
                .ToList();
            return new JsonResult(drives);
        }

        public IActionResult OnGetFolders(string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path)
                    .Select(d => new { name = Path.GetFileName(d), path = d })
                    .ToList();
                return new JsonResult(directories);
            }
            catch
            {
                return new JsonResult(new object[] { });
            }
        }

        public async Task<IActionResult> OnPostRestoreAsync()
        {
            if (RestoreFile == null || RestoreFile.Length == 0)
            {
                ErrorMessage = "Please select a valid backup file.";
                return Page();
            }

            if (!Path.GetExtension(RestoreFile.FileName).Equals(".bak", StringComparison.OrdinalIgnoreCase))
            {
                ErrorMessage = "Only .bak files are allowed.";
                return Page();
            }

            string tempFilePath = Path.GetTempFileName();
            
            try
            {
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await RestoreFile.CopyToAsync(stream);
                }

                string dbPath = Path.Combine(_env.ContentRootPath, "Data", "Database1.mdf");
                string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
                
                string databaseName = "";
                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    databaseName = connection.Database;
                }

                string masterConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
                using (var masterConn = new SqlConnection(masterConnStr))
                {
                    await masterConn.OpenAsync();

                    using (var cmd = new SqlCommand($"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", masterConn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                    using (var cmd = new SqlCommand($"RESTORE DATABASE [{databaseName}] FROM DISK = @backupFile WITH REPLACE", masterConn))
                    {
                        cmd.Parameters.AddWithValue("@backupFile", tempFilePath);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    using (var cmd = new SqlCommand($"ALTER DATABASE [{databaseName}] SET MULTI_USER", masterConn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                Message = "Database restored successfully.";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error during restore: " + ex.Message;
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath))
                {
                    try { System.IO.File.Delete(tempFilePath); } catch { }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBackupAsync()
        {
            if (string.IsNullOrEmpty(BackupPath))
            {
                ErrorMessage = "Please specify a backup path.";
                return Page();
            }

            if (!Directory.Exists(BackupPath))
            {
                Directory.CreateDirectory(BackupPath);
            }

            string backupFileName = Path.Combine(BackupPath, $"Database-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.bak");

            try
            {
                // We need to get the database name from the connection
                using (var connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Path.Combine(_env.ContentRootPath, "Data", "Database1.mdf") + ";Integrated Security=True"))
                {
                    await connection.OpenAsync();
                    string database = connection.Database;
                    string query = $"BACKUP DATABASE [{database}] TO DISK = @backupPath";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@backupPath", backupFileName);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                Message = $"Backup taken successfully to: {backupFileName}";
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error during backup: " + ex.Message;
            }

            return Page();
        }
    }
}
