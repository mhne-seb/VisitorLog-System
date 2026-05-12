# Visitor Log System — ASP.NET Core MVC

A complete Visitor Log System with full CRUD functionality, session-based authentication, and Microsoft SQL Server.

---

## Tech Stack

| Layer       | Technology                              |
|-------------|----------------------------------------|
| Framework   | ASP.NET Core 8.0 MVC                   |
| Database    | Microsoft SQL Server (via EF Core)     |
| ORM         | Entity Framework Core 8                |
| Auth        | Session-based (no Identity)            |
| UI          | Bootstrap 5.3 + Bootstrap Icons        |
| Font        | Plus Jakarta Sans (Google Fonts)       |

---

## Project Structure

```
VisitorLog/
├── Controllers/
│   ├── AccountController.cs      # Login / Logout
│   └── HomeController.cs         # CRUD for Visitors
├── Data/
│   └── AppDbContext.cs           # EF Core DbContext
├── Models/
│   ├── Visitor.cs                # Visitor entity
│   └── LoginViewModel.cs         # Login form model
├── Views/
│   ├── Account/
│   │   └── Login.cshtml          # Login page
│   ├── Home/
│   │   └── Index.cshtml          # Single-panel dashboard
│   ├── Shared/
│   │   └── _Layout.cshtml        # Master layout
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── appsettings.json              # ← Set your SQL Server here
├── Program.cs
└── VisitorLog.csproj
```

---

## Quick Setup

### Step 1 — Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (Local, Express, or full)
- Visual Studio 2022 / VS Code / Rider

### Step 2 — Configure the Connection String

Open `appsettings.json` and update:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=VisitorLogDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Common server names:**

| Setup                  | Server Name              |
|------------------------|--------------------------|
| SQL Server Express     | `.\SQLEXPRESS`           |
| LocalDB                | `(localdb)\MSSQLLocalDB` |
| SQL Server (default)   | `.` or `localhost`       |
| Named instance         | `.\INSTANCENAME`         |

### Step 3 — Run the App

```bash
cd VisitorLog
dotnet restore
dotnet run
```

The app automatically creates the `VisitorLogDB` database and the `Visitors` table via `db.Database.EnsureCreated()` on startup.

### Step 4 — Login

Navigate to `https://localhost:PORT/Account/Login`

| Field    | Value      |
|----------|------------|
| Username | `admin`    |
| Password | `admin123` |

---

## Features

### Authentication
- Hardcoded single admin login
- Session-based authentication (30-minute timeout)
- All dashboard routes redirect to login if not authenticated
- Logout button in topbar

### Visitor CRUD
| Action  | How                                        |
|---------|--------------------------------------------|
| Add     | Fill form at the top of dashboard → Save   |
| View    | Table at the bottom of dashboard           |
| Search  | Search bar — searches name, purpose, person, contact |
| Edit    | Click ✏️ Edit button → modal form opens    |
| Delete  | Click 🗑️ Delete → confirmation modal      |

### Visitor Fields
- ID (auto-generated)
- Full Name
- Purpose of Visit
- Person to Visit
- Contact Number
- Date and Time Visited (auto-set on create, editable on update)

---

## Manual EF Core Migration (Optional)

If you prefer proper migrations over `EnsureCreated`:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Then remove `db.Database.EnsureCreated()` from `Program.cs`.

---

## SQL Server Manual Setup (Alternative)

If you prefer to create the database manually:

```sql
CREATE DATABASE VisitorLogDB;
GO

USE VisitorLogDB;
GO

CREATE TABLE Visitors (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100)  NOT NULL,
    PurposeOfVisit  NVARCHAR(200)  NOT NULL,
    PersonToVisit   NVARCHAR(100)  NOT NULL,
    ContactNumber   NVARCHAR(20)   NOT NULL,
    DateTimeVisited DATETIME2      NOT NULL DEFAULT GETDATE()
);
GO
```

---

## UI Highlights

- **Single-panel dashboard** — Add form (top) + Visitor table (bottom)
- **No sidebar** — clean, distraction-free layout
- **Edit via Bootstrap modal** — inline, no page redirect
- **Delete with confirmation modal** — prevents accidental deletes
- **Auto-dismiss success/error alerts** — fade out after 4 seconds
- **Animated table rows** — staggered fade-in on load
- **Responsive** — works on mobile, tablet, and desktop
- **Professional topbar** — brand + admin badge + logout

---

## Changing Admin Credentials

Edit `Controllers/AccountController.cs`:

```csharp
private const string AdminUsername = "admin";
private const string AdminPassword = "admin123";
```

> For production, store these in `appsettings.json` or use environment variables.
