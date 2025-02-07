using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Delivery.Infrastructure.Persistence;

public class DeliveryDbContextFactory : IDesignTimeDbContextFactory<DeliveryDbContext>
{
    public DeliveryDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DeliveryDbContext>();

        Console.WriteLine("Args: " + args.Aggregate("", (a, b) => a + Environment.NewLine + b));

        var dbConnectionString = args.FirstOrDefault();
        if (dbConnectionString == null) throw new Exception("Invalid CLI argument connection string");

        optionsBuilder.UseNpgsql(dbConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();

        return new DeliveryDbContext(optionsBuilder.Options, null, null);
    }
}