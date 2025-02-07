using Contracts.Events;
using Contracts.Queries;
using SharedKernel;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Domain.Models;

public class Plan : AggregateRoot<PlanId>
{
    // vehicle, courier, packages, route, currentPackageIndex, status
    
    public Guid VehicleId { get; private set; }
    public Guid CourierId { get; private set; }
    public List<Guid> PackageIds { get; private set; }
    public int CurrentPackageIndex { get; private set; } = 0;
    public Route Route { get; private set; }
    public PlanStatusEnum Status { get; private set; } = PlanStatusEnum.Created;
    public DateTime? StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    
    public static ErrorOr<Plan> Create()
    {
        return new Plan
        {
            //TODO get - Warehouse/APP/CommandHandlers OR CONTRACTS/Queries
            VehicleId = GetAvailableCourierQuery(),
            CourierId = GetAvailableCourierQuery(),
            PackageIds = GetPackagesForDeliveryQuery(VehicleId),
            //TODO set vehicle, courier and packages statuses OR CONTRACTS/Queries OR Events
            DeliveryStartedEvent(VehicleId, CourierId, PackageIds);
            //TODO create a route OR CONTRACTS/Command
            //TODO origin set as vehicle location
            //TODO get destinations from packages
            Route = RoutePackagesCommand(Destinations, origin);
        };
    }
    
    public void UpdateStatus(PlanStatusEnum status)
    {
        Status = status;
        
        switch (Status)
        {
            case PlanStatusEnum.InDelivery:
                StartedAt = DateTime.Now.ToUniversalTime();
                break;
            case PlanStatusEnum.Delivered:
                EndedAt = DateTime.Now.ToUniversalTime();
                break;
        }
    }

    //TODO annotation
    public Guid? ProceedToNextPackage(bool currPackageDeliverySuccessul)
    {
        // TODO what to use? UpdatePackageRequest - curr OR CONTRACTS/Events
        // TODO what to use? UpdatePackageRequest - next at CONTRACTS/Events

        CurrentPackageIndex += 1;
        
        if (CurrentPackageIndex >= PackageIds.Count)
        {
            UpdateStatus(PlanStatusEnum.Delivered);
            DeliveryEndedEvent(VehicleId, CourierId);
            return null;
        }

        return PackageIds[CurrentPackageIndex];
    }

    public ErrorOr<Success> Update(
        Guid? vehicleId, 
        Guid? courierId, 
        List<Guid>? packageIds,
        string? status)
    {
        // TODO check if ids are existing
       VehicleId = vehicleId ?? VehicleId;
       CourierId = courierId ?? CourierId;
       PackageIds = packageIds ?? PackageIds;
        
        if (status != null)
        {
            if (!Enum.TryParse<PlanStatusEnum>(status, true, out var newStatus))
                return Error.Validation("Invalid status");
            UpdateStatus(newStatus);
        }
        
        return Result.Success;
    }
}