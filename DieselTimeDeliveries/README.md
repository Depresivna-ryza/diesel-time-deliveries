running migrations: 
```bash
POSTGRES_CONNECTION_STRING="Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres" dotnet ef migrations add init --project Warehouse --startup-project DieselTimeDeliveries
```

applying migrations
```bash
DB_CONNECTION_STRING="Host=localhost;Port=5432;Database=juiceworld;Username=postgres;Password=postgres" dotnet ef database update --project Warehouse --startup-project DieselTimeDeliveries
```