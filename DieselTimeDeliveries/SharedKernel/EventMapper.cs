using Inventory.Contracts.IntegrationEvents;
using Inventory.Domain.Order.DomainEvents;
using SharedKernel;

namespace Inventory.Domain.Common;

public static class EventMapper
{
    public static IntegrationEvent? MapToIntegrationEvent(this DomainEvent @event)
    {
        return @event switch
        {
            IOrderDomainEvent orderEvent => OrderDomainEventsMapper.MapToIntegrationEvent(orderEvent),
            _ => throw new InvalidOperationException($"No mapper defined for {@event}")
        };
    }
}