using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Persistence;

public static class DataInitializer
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Courier>().HasData(
            new Courier { id = Guid.NewGuid() },
            new Courier { id = Guid.NewGuid() },
            new Courier { id = Guid.NewGuid() },
            new Courier { id = Guid.NewGuid() }
        );
    }
}