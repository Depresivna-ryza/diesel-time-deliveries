using SharedKernel;
using SharedKernel.Interfaces;

namespace Warehouse.Domain.Events;

public class PackageDomainEvent(Guid id) : IDomainEvent
{
    public IIntegrationEvent MapToIntegrationEvent()
    {
        return new PackageIntegrationEvent() {Id = id}; //this is an example for future use
    }
}