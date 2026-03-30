## 🚀 How to Run the Project

### Prerequisites
- Visual Studio (ASP.NET workload) OR .NET SDK installed
- SQL Server + SSMS

### Steps

1. Clone the repository  
   git clone https://github.com/<your-username>/<repo-name>.git

2. Setup Database  
   - Create DB in SQL Server  
   - Run provided SQL script  

3. Update Connection String  
   - Edit `Web.config`

### ▶ Run using Visual Studio (Recommended)
- Open `.sln` file  
- Press **Ctrl + F5**

### ▶ Run using .NET CLI (Terminal)
```bash
dotnet build
dotnet run
