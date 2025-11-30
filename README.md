# Diesel Time Deliveries

This is an implementation of the web back-end of a package delivery service with a focus on a modern software architecture approach. It consists of a modular monolith, each with its own Clean Architecture. Focus is on the usage of various modern tools, like Aspire, Wolverine messaging, Entity Framework, Postgres, Docker, ...



### Running Migrations
Make sure to have the database running with the provided Docker.


```bash
POSTGRES_CONNECTION_STRING="Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres" dotnet ef migrations add init --project Warehouse --startup-project DieselTimeDeliveries -- 'Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres'
```

applying migrations
```bash
POSTGRES_CONNECTION_STRING="Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres" dotnet ef database update --project Warehouse --startup-project DieselTimeDeliveries -- 'Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres'
```

drop database
```bash
POSTGRES_CONNECTION_STRING="Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres" dotnet ef database drop --project Warehouse --startup-project DieselTimeDeliveries -- 'Host=localhost;Port=5432;Database=DTD;Username=postgres;Password=postgres'
```

### Usage

```
dotnet run
```
Or run with an IDE
