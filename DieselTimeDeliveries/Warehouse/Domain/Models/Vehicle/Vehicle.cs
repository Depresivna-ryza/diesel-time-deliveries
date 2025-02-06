using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel;
using ErrorOr;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Warehouse.Domain.Events;
using Warehouse.Domain.Models.Package;

namespace Warehouse.Domain.Models.Vehicle;

public class Vehicle : AggregateRoot<VehicleId>
{
    public string Make { get; private set; }
    public string Model { get; private set; }
    public Weight PackageWeightLimit { get; private set; }
    
    public Vin Vin { get; private set; }
    public VehicleStatusEnum Status { get; private set; }

    public static ErrorOr<Vehicle> Create(string make, string model, decimal weightLimit, string vin)
    {
        if (string.IsNullOrWhiteSpace(make))
            return Error.Validation("invalid vehicle make");
        
        if (string.IsNullOrWhiteSpace(model))
            return Error.Validation("invalid vehicle model");
        
        
        var weightOrError = Weight.Create(weightLimit);
        if (weightOrError.IsError)
        {
            return weightOrError.Errors;
        }
        
        
        var vinOrError = Vin.Create(vin);
        if (vinOrError.IsError)
        {
            return vinOrError.Errors;
        }

        return new Vehicle {
            Id = VehicleId.CreateUnique(),
            Make = make,
            Model = model,
            PackageWeightLimit = weightOrError.Value,
            Vin = vinOrError.Value,
            Status = VehicleStatusEnum.Available
        };
    }

    // public void VehicleAdded() 
    // {
    //     RaiseEvent(new VehicleDomainEvent(Id.Value));
    // }
}