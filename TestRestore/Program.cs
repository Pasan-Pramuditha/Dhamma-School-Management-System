using System;
using Microsoft.Data.SqlClient;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string dbPath = Path.GetFullPath(@"..\SiriVajiranana_Web\Data\Database1.mdf");
        string backupDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Backups");
        Directory.CreateDirectory(backupDir);
        string backupFile = Path.Combine(backupDir, "TestBackup.bak"); 

        string connStr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True";
        
        string dbName = "";
        using (var conn = new SqlConnection(connStr))
        {
            conn.Open();
            dbName = conn.Database;
            Console.WriteLine($"Database Name: {dbName}");
            var backupCmd = new SqlCommand($"BACKUP DATABASE [{dbName}] TO DISK = '{backupFile}' WITH INIT", conn);
            backupCmd.ExecuteNonQuery();
            Console.WriteLine("Backup created at: " + backupFile);
        }

        string masterConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
        using (var conn = new SqlConnection(masterConnStr))
        {
            conn.Open();
            Console.WriteLine($"Connected to master. Attempting to restore {dbName}");
            try {
                var cmd = new SqlCommand($"ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", conn);
                cmd.ExecuteNonQuery();
                
                cmd = new SqlCommand($"RESTORE DATABASE [{dbName}] FROM DISK = '{backupFile}' WITH REPLACE", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand($"ALTER DATABASE [{dbName}] SET MULTI_USER", conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Restore completed.");
            } catch (Exception ex) {
                Console.WriteLine("Restore failed: " + ex.Message);
            }
        }
    }
}
