# Plataforma de Créditos

## Requisitos locales

- .NET 8 SDK
- SQLite (incluido en EF Core)

## Ejecutar localmente

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Usuarios de prueba

| Email                 | Password     | Rol      |
| --------------------- | ------------ | -------- |
| analista@creditos.com | Analista123! | Analista |
| cliente1@creditos.com | Cliente123!  | Cliente  |
| cliente2@creditos.com | Cliente123!  | Cliente  |

## Variables de entorno (Render)

| Variable                               | Valor                         |
| -------------------------------------- | ----------------------------- |
| ASPNETCORE_ENVIRONMENT                 | Production                    |
| ASPNETCORE_URLS                        | http://0.0.0.0:${PORT}        |
| ConnectionStrings\_\_DefaultConnection | Data Source=/data/creditos.db |

## URL de Render

https://plataformacreditos-9sqh.onrender.com
