using SharedKernel.Interfaces;

namespace Warehouse.Domain.Events;

public class PackageIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; set; }
}