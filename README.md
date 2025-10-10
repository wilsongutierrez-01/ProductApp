# ProductApp

Aplicación de ejemplo para gestión de productos usando **ASP.NET Core**, **Entity Framework Core** y **SQL Server**.

## Requisitos

- .NET 8 SDK o superior
- SQL Server (local o remoto)
- Visual Studio, Rider o VS Code

---

## Configuración de la base de datos

### 1. Crear la base de datos

Ejecuta estas consultas en tu SQL Server:

```sql
CREATE DATABASE ProductAppDb;
```

##Configuración en ```appsettings.json```, agregar el puerto correcto segun el servidor, y el nombre del host
```
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1435;Database=ProductAppDb;User Id=sa;Password=root12345@Password;Encrypt=False;TrustServerCertificate=True;"
  },
```
