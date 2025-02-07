using System.Reflection;
using Delivery.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Extensions;
using Wolverine.Attributes;

[assembly: WolverineModule]

namespace Delivery;

public static class DeliveryInstaller
{
    public static IServiceCollection DeliveryInstall(this IServiceCollection services, string inventoryConnectionString)
    {
        services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());
        services.AddDbContextFactory<DeliveryDbContext>(options =>
        {
            options.UseNpgsql(inventoryConnectionString);
        });
        
        return services;
    }
}