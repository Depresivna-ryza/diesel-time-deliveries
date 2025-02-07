using Contracts.Commands;
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
    public int CurrentPackageIndex { get; private set; } = -1;
    public Route Route { get; private set; }
    public PlanStatusEnum Status { get; private set; } = PlanStatusEnum.Created;
    public DateTime? StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    
    public static ErrorOr<Plan> Create()
    {
        var vehicleId = GetAvailableCourierQuery();
        var courierId = GetAvailableCourierQuery();
        var packageIds = GetPackagesForDeliveryQuery(vehicleId);
        
        var origin = "Sumavska 68/a, 602 00 Brno, Czechia"; //future extension: get current vehicle location
        var route = RoutePackagesCommand(GetAddressesFromPackageIds(packageIds), origin);
        
        DeliveryStartedEvent(vehicleId, courierId, packageIds);
        
        return new Plan
        {
            VehicleId = vehicleId,
            CourierId = courierId,
            PackageIds = packageIds,
            Route = route
        };
    }
    
    public static List<string> GetAddressesFromPackageIds(List<Guid> packageIds)
    {
        var addresses = new List<string>();

        foreach (var packageId in packageIds)
        {
            var package = GetPackageById(packageId); //TODO add Contract/Query
            if (package != null)
            {
                addresses.Add(package.Address); 
            }
        }

        return addresses;
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
        PackageDeliveredEvent(PackageIds[CurrentPackageIndex], currPackageDeliverySuccessul);

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
        // Future extension: check if ids are existing
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