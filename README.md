# 🎓 StudentPortal

**StudentPortal** is a full-stack ASP.NET Core MVC web application designed to streamline assignment submission, student assessment, and faculty-student interaction within the university.

---

## 🚀 Features

- 🧑‍🏫 **Role-based Access** (Faculty / Student)
- 📝 **Assignment Management**
  - Faculty can create and assign tasks
  - Students can submit assignments
- 📊 **Assessment & Grading**
  - Faculty can assess students using predefined criteria
  - Automated score calculation
- 📈 **Performance Visualization**
  - Graphical performance charts using Chart.js / Power BI
- 🔐 **Authentication**
  - Secure login, registration & password change with session support
- 📁 **Modular Architecture**
  - Clean separation of concerns via Services, Repositories & Interfaces
- 💡 **Optional AI Integration**
  - Enhance assessments or suggestions using AI APIs (planned)

---

## 🛠️ Tech Stack

| Layer           | Technology                     |
|----------------|---------------------------------|
| Frontend        | Razor Pages, Bootstrap, jQuery |
| Backend         | ASP.NET Core MVC (.NET 8)      |
| Database        | SQL Server (Stored Procedures via Dapper) |
| Charts          | Chart.js            |
| Hosting         | Azure App Services / SQL DB    |

---

## 🧱 Project Structure

StudentPortal/
│
├── StudentPortal.Web # Web UI (MVC)
├── StudentPortal.Business # Business layer (Services & Interfaces)
├── StudentPortal.Data # Data layer (Repositories, DapperContext)
├── StudentPortal.Models # Entity Models
├── StudentPortal.sln # Solution File
└── README.md


## 📦 Setup Instructions

### 1. Prerequisites

- .NET SDK 8+
- SQL Server (Local/Cloud)
- Visual Studio or VS Code
