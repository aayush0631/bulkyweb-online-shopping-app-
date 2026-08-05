# FileSync - Automated File Synchronization Tool

FileSync is a .NET 8 MVC web application for managing and scheduling file copy operations between Windows network shares (SMB) and local directories. It provides a web dashboard to configure credentials, define sync tasks with scheduling, and runs copies in the background via a Worker service.

---

## 🏗️ Solution Architecture

```
FileSync.sln
├── FileSync.Models          # Domain models, ViewModels, DTOs
├── FileSync.DataAccess      # EF Core DbContext, Repositories, Migrations
├── FileSync.Services        # Business logic, Background services
├── FileSync.Utilities       # Constants and helper utilities
├── FileSync.Web             # ASP.NET Core MVC Web UI (Controllers + Views)
├── FileSync.Worker          # Background Worker Service (runs scheduled tasks)
└── FileSync.Tests           # Unit tests project
```

### Layer Diagram

```
┌─────────────────────────────────────────────┐
│              FileSync.Web (MVC)             │
│  Controllers ─── Views ─── Program.cs       │
└────────────────┬────────────────────────────┘
                 │  depends on
┌────────────────▼────────────────────────────┐
│           FileSync.Services                 │
│  CredentialService, SyncTaskService,        │
│  FileCopyService, NetworkConnectionService, │
│  SchedulerService, EncryptionService        │
└────────────────┬────────────────────────────┘
                 │  depends on
┌────────────────▼────────────────────────────┐
│          FileSync.DataAccess                │
│  ApplicationDbContext, UnitOfWork,          │
│  Repository<T>, SyncTaskRepo, CredentialRepo│
└────────────────┬────────────────────────────┘
                 │  depends on
┌────────────────▼────────────────────────────┐
│          FileSync.Models                    │
│  SyncTask, Credential, Schedule,            │
│  CopyHistory, ViewModels                    │
└─────────────────────────────────────────────┘
```

---

## 📦 Domain Models

| Model | Description |
|---|---|
| **Credential** | Stores connection info: `ConnectionName`, `Protocol` (SMB/FTP), `ServerName`, `Port`, `ShareName` (optional for FTP), `UserName`, `Password`, `IsActive` |
| **SyncTask** | Defines a file copy job: `TaskName`, `RemoteRelativePath`, `LocalPath`, copy options (`SkipIfExists`, `ResumeIfInterrupted`, `VerifyAfterCopy`), `IsEnabled`. FK to `Credential` and `Schedule` |
| **Schedule** | Stores scheduling config: `StartTime`, `RepeatDaily`, `RepeatWeekly`, `RepeatMonthly`, `IsEnabled` |
| **CopyHistory** | Logs each copy run: `SyncTaskId`, `StartedAt`, `CompletedAt`, `Success`, `ErrorMessage`, `BytesCopied` |

---

## 🔧 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express)
- [EF Core CLI Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

---

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <your-repo-url>
cd FileSync
```

### 2. Configure the Database Connection
Edit `FileSync.Web/appsettings.json` and set your SQL Server connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FileSync;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Apply Database Migrations
```bash
dotnet ef database update --startup-project FileSync.Web --project FileSync.DataAccess
```

### 4. Build the Solution
```bash
dotnet build
```

### 5. Run the Web Application
```bash
dotnet run --project FileSync.Web
```
Navigate to **http://localhost:5209** in your browser.

---

## 📋 How to Use the Application

### Managing Credentials (Network Connections)
1. Click **Credentials** in the navigation bar.
2. Click **Add Credential** to create a new connection.
3. Fill in:
   - **Connection Name** – a friendly label (e.g. "Backup VPS", "Phone FTP")
   - **Protocol** – Select **SMB** or **FTP**
   - **Server Name / IP** – the hostname or IP of the remote server
   - **Port** – The port (e.g., `21` or a custom high port like `6587` for mobile FTP servers)
   - **Share Name** – (*SMB Only*) the share directory name on Windows network
   - **Username / Password** – authentication details
4. Click **Create**. The credential is saved and the password is encrypted.
5. Use the ✏️ (Edit) or 🗑️ (Delete) buttons to manage existing credentials.

### Managing Sync Tasks
1. Click **Sync Tasks** in the navigation bar.
2. Click **Add Sync Task** to create a new file sync job.
3. Fill in:
   - **Task Name** – a descriptive name (e.g. "Backup Photos")
   - **Connection** – select one of your saved credentials from the dropdown
   - **Remote Relative Path** – the subfolder on the remote share/FTP (e.g. `/device/Download/MyResume.pdf`)
   - **Local Path** – the local directory to sync files to (e.g. `C:\Backups\MyResume.pdf`)
   - **Schedule Time** – when the task should run
   - **Copy Options** – toggle Skip If Exists, Resume Interrupted Copy, Verify After Copy
   - **Enabled** – whether the task is active
4. Click **Create**.
5. On the Sync Tasks index page, you can:
   - ⏯️ **Toggle** enable/disable
   - ✏️ **Edit** task settings
   - 🗑️ **Delete** the task

---

## 🧪 Running Tests

```bash
dotnet test FileSync.Tests\FileSync.Tests.csproj
```

---

## 📂 Key Files Reference

| File | Purpose |
|---|---|
| `FileSync.Models/Models/Credential.cs` | Credential entity with Protocol (enum) and Port support |
| `FileSync.Models/Models/SyncTask.cs` | Sync task entity with copy options and FK relationships |
| `FileSync.Models/ViewModels/CredentialViewModel.cs` | ViewModel for Credential CRUD forms |
| `FileSync.Models/ViewModels/SyncTaskViewModel.cs` | ViewModel for SyncTask CRUD forms |
| `FileSync.DataAccess/Data/ApplicationDbContext.cs` | EF Core DbContext with DbSets |
| `FileSync.DataAccess/Repository/UnitOfWork.cs` | Unit of Work pattern implementation |
| `FileSync.Services/Implementations/CredentialService.cs` | Credential business logic + encryption |
| `FileSync.Services/Implementations/SyncTaskService.cs` | SyncTask business logic |
| `FileSync.Services/Implementations/NetworkConnectionService.cs` | SMB network connection via Win32 API |
| `FileSync.Services/Implementations/FtpConnectionService.cs` | FTP network connection validation |
| `FileSync.Services/Implementations/FileCopyService.cs` | Core SMB/local file copy worker |
| `FileSync.Services/Implementations/FtpFileCopyService.cs` | Core FTP binary file copy & resume engine |
| `FileSync.Services/Implementations/ConnectionServiceFactory.cs` | Factory resolving INetworkConnectionService at runtime |
| `FileSync.Services/Implementations/FileCopyServiceFactory.cs` | Factory resolving IFileCopyService at runtime |
| `FileSync.Web/Program.cs` | DI registration and middleware pipeline with factories |
| `FileSync.Web/Controllers/CredentialController.cs` | CRUD controller for credentials |
| `FileSync.Web/Controllers/SyncTaskController.cs` | CRUD + Toggle controller for sync tasks |

---

## 🔐 Security Notes

- Passwords are encrypted at rest using `EncryptionService` before being stored in the database.
- Credentials are transmitted over HTTPS when the app is run in production mode.
- Connection strings should be stored in environment variables or Azure Key Vault in production.

---

## 📝 License

This project is for internal/educational use.
