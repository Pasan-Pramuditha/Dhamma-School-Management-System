# Siri Vajiranana Dhamma School Management System

A modern web application built with **ASP.NET Core (.NET 9 Razor Pages)** and **SQL Server LocalDB** to manage students, teachers, results, and administrative accounts.

---

## 🚀 Features
- **Student Management:** Register, update, search, and view student details and academic marks across grades 5–11.
- **Teacher Management:** Register, update, and manage teacher profiles and subjects.
- **Role-based Admin Control:**
  - **Super Admin:** Automatically granted to the very first registered user. Has full authority to approve or reject pending admin registrations.
  - **Standard Admin:** Requires Super Admin approval before logging in.
- **Automated Database Creation:** The database and tables are generated automatically on startup if not present.
- **Backup & Restore:** Easy database backup and restore functionality.

---

## 📋 Prerequisites
Ensure you have the following installed on your machine:
1. **[.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)** (or later)
2. **Microsoft SQL Server LocalDB** (Included with Visual Studio under the *.NET desktop development* or *Data storage and processing* workload, or downloadable via SQL Server Express LocalDB).

---

## 🛠️ Getting Started (How to Run After Cloning)

### 1. Clone the Repository
```bash
git clone https://github.com/Pasan-Pramuditha/Dhamma-School-Management-System.git
cd Dhamma-School-Management-System
```

### 2. Run the Application
You can run the application using either the .NET CLI or Visual Studio:

#### Option A: Using .NET CLI (Terminal / Command Prompt / PowerShell)
```bash
dotnet run --project SiriVajiranana_Web
```
Once started, open your web browser and navigate to the local URL displayed (e.g., `https://localhost:7087` or `http://localhost:5087`).

#### Option B: Using Visual Studio
1. Open `SiriVajiranana.sln` in Visual Studio 2022.
2. Set `SiriVajiranana_Web` as the Startup Project.
3. Press **F5** or click the green **Run** button.

---

## 🗄️ Database Initialization (Automatic)
- You **do NOT need to manually create the database**.
- When the project runs for the first time, `Program.cs` automatically invokes `DatabaseService.EnsureDatabaseCreatedAsync()`.
- If `Database1.mdf` does not exist inside `SiriVajiranana_Web/Data/`, the system connects to LocalDB, creates `Database1.mdf`, and creates the required database tables:
  - `Login`
  - `Student_Registration`
  - `Teachers`

---

## 👑 First-Time Setup: Creating the Super Admin

Because database binary files (`.mdf` / `.ldf`) are excluded from GitHub for security and portability, the initial database will be empty. Follow these simple steps to set up your Super Admin account:

1. Open the application in your browser (`/Account/Login`).
2. Click on **"Don't have an account? Sign Up"** (or visit `/Account/SignUp`).
3. Enter your details:
   - **Admin Name** (e.g., `Super Admin` or your name)
   - **Username**
   - **Password**
4. Click **Register**.
5. **System Initialized!** Because this is the very first registration in the system, you are **automatically registered as Super Admin** (`AdminID = 1`, `IsApproved = 1`) with immediate access.
6. You will be redirected to the Login page with a success message. Enter your username and password to log in to the Dashboard!

> 💡 **Subsequent Registrations:** Any user who registers after the Super Admin will be in `Pending Approval` state (`IsApproved = 0`). The Super Admin can approve or decline them directly from the Admin Dashboard.

---

## 📄 Manual Database Setup (Optional)
If you prefer to manually initialize the database using **SQL Server Management Studio (SSMS)** or **Visual Studio SQL Object Explorer**:
1. Open the file `SiriVajiranana_Web/Data/ResetDb.sql`.
2. Connect to your SQL Server / LocalDB instance (`(LocalDB)\MSSQLLocalDB`).
3. Execute the script to drop/recreate tables and optionally seed a default Super Admin.

---

## 📁 Project Structure
```
Dhamma-School-Management-System/
├── SiriVajiranana.sln
├── README.md
├── .gitignore
└── SiriVajiranana_Web/
    ├── Data/
    │   ├── DatabaseService.cs      # Database connection & auto-migration
    │   ├── ResetDb.sql             # SQL script for table schemas
    │   └── CreateTeachersTable.sql
    ├── Models/                     # Data models (Student, Teacher, Admin)
    ├── Pages/                      # Razor Pages (Account, Admin, Students, Teachers)
    ├── Program.cs                  # Web application entry point
    └── wwwroot/                    # CSS, JS, Images, Libraries
```
