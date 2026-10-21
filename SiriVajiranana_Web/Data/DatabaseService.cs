using Microsoft.Data.SqlClient;
using System.Data;

namespace SiriVajiranana_Web.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly string _contentRootPath;

        public DatabaseService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;
            // Set the DataDirectory for the connection string
            string dbPath = Path.Combine(env.ContentRootPath, "Data");
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }
            AppDomain.CurrentDomain.SetData("DataDirectory", dbPath);
            
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? 
                                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            string dataDir = Path.Combine(_contentRootPath, "Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            string mdfPath = Path.Combine(dataDir, "Database1.mdf");

            // If .mdf file does not exist, create it using master
            if (!File.Exists(mdfPath))
            {
                string masterConnStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";
                using (var masterConn = new SqlConnection(masterConnStr))
                {
                    await masterConn.OpenAsync();
                    using (var cmd = masterConn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Database1')
                            BEGIN
                                ALTER DATABASE [Database1] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                                DROP DATABASE [Database1];
                            END";
                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = $"CREATE DATABASE [Database1] ON (NAME = 'Database1', FILENAME = '{mdfPath}')";
                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = "EXEC sp_detach_db 'Database1'";
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            // Ensure all tables exist
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"
                        IF OBJECT_ID('dbo.Login', 'U') IS NULL
                        BEGIN
                            CREATE TABLE Login (
                                AdminID int PRIMARY KEY,
                                UserName varchar(50) NOT NULL,
                                Password varchar(50) NOT NULL,
                                Admin_Name nvarchar(255) NOT NULL,
                                IsApproved bit DEFAULT 0
                            );
                        END

                        IF OBJECT_ID('dbo.Student_Registration', 'U') IS NULL
                        BEGIN
                            CREATE TABLE Student_Registration (
                                Registration_No int PRIMARY KEY,
                                Name varchar(255),
                                Gender varchar(20),
                                Address varchar(255),
                                Telephone_No varchar(20),
                                Other_Skills nvarchar(MAX),
                                Year_of_Admission varchar(50),
                                Grade05_Buddhism varchar(255),
                                Grade05_Abhidhamma varchar(255),
                                Grade05_Pali varchar(255),
                                Grade06_Buddhism varchar(255),
                                Grade06_Abhidhamma varchar(255),
                                Grade06_Pali varchar(255),
                                Grade07_Buddhism varchar(255),
                                Grade07_Abhidhamma varchar(255),
                                Grade07_Pali varchar(255),
                                Grade08_Buddhism varchar(255),
                                Grade08_Abhidhamma varchar(255),
                                Grade08_Pali varchar(255),
                                Grade09_Buddhism varchar(255),
                                Grade09_Abhidhamma varchar(255),
                                Grade09_Pali varchar(255),
                                Grade10_Buddhism varchar(255),
                                Grade10_Abhidhamma varchar(255),
                                Grade10_Pali varchar(255),
                                Grade11_Buddhism varchar(255),
                                Grade11_Abhidhamma varchar(255),
                                Grade11_Pali varchar(255)
                            );
                        END

                        IF OBJECT_ID('dbo.Teachers', 'U') IS NULL
                        BEGIN
                            CREATE TABLE Teachers (
                                TeacherID int PRIMARY KEY,
                                Name nvarchar(255) NOT NULL,
                                Gender varchar(20) NOT NULL,
                                NIC varchar(20) NOT NULL,
                                Telephone_No varchar(20) NOT NULL,
                                Address nvarchar(MAX) NOT NULL,
                                Email nvarchar(100),
                                Year_of_Appointment varchar(20) NOT NULL,
                                Subject nvarchar(100) NOT NULL
                            );
                        END";
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }
    }
}
