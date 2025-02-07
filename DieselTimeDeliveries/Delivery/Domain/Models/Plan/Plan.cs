using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Warehouse.Domain.Models.Courier;
using Warehouse.Domain.Models.Package;
using Warehouse.Domain.Models.Vehicle;
using ErrorOr;
using Warehouse.Application.EndpointHandlers.Package;
using Warehouse.Presentation.Package;

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
            //TODO get - Warehouse/APP/CommandHandlers
            //VehicleId = vehicleId,
            //CourierId = courierId,
            //PackageIds = packagesId,
            //TODO set vehicle, courier and packages statuses
            //TODO create a route
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
        // TODO what to use? UpdatePackageRequest - curr
        // TODO what to use? UpdatePackageRequest - next

        CurrentPackageIndex += 1;
        
        if (CurrentPackageIndex >= PackageIds.Count)
        {
            UpdateStatus(PlanStatusEnum.Delivered);
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