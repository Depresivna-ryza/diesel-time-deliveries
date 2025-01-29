using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Extensions;
using Warehouse.Infrastructure.Persistence;
using Wolverine.Attributes;

[assembly: WolverineModule]

namespace Warehouse;

public static class WarehouseInstaller
{
    public static IServiceCollection InstallInventory(this IServiceCollection services, string inventoryConnectionString)
    {
        services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());
        
        services.AddDbContext<WarehouseDbContext>(options =>
        {
            options.UseNpgsql(inventoryConnectionString);
        });
        
        return services;
    }
    
    public static void SeedInventory(WarehouseDbContext context)
    {
        
        
    }
}