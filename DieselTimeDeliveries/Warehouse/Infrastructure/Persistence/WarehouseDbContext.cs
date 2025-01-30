using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Warehouse.Domain.Models;
using Warehouse.Domain.Models.Package;
using Wolverine;

namespace Warehouse.Infrastructure.Persistence;

public class WarehouseDbContext : DbContext {
    
    private readonly IMessageBus _sender;
    
    public DbSet<Package> Packages { get; set; } = null!;
    public DbSet<Courier> Couriers { get; set; } = null!;

    // public WarehouseDbContext() {    } ?????????????????

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options, IMessageBus sender) : base(options)
    {
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        _sender = sender;
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Seed();
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishEventsAsync();
        
        return result;
    }
    
    private async Task PublishEventsAsync()
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e =>
            {
                var events = e.DomainEvents;
                
                e.ClearEvents();
                
                return events;
            }).ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _sender.PublishAsync(domainEvent);

            var integrationEvent = domainEvent.MapToIntegrationEvent();
            if (integrationEvent is not null)
                await _sender.PublishAsync(integrationEvent);
        }
    }
}