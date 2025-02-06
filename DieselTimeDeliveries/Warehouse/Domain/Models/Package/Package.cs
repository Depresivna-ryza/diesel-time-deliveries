using ErrorOr;
using SharedKernel;
using Warehouse.Domain.Events;

namespace Warehouse.Domain.Models.Package;

public class Package : AggregateRoot<PackageId>
{
    public string Name { get; private set; }
    public Weight Weight { get; private set; }
    public string Destination { get; private set; }
    public PackageStatusEnum Status { get; private set; }

    public static ErrorOr<Package> Create(string name, decimal weight, string destination)
    {
        var weightOrError = Weight.Create(weight);
        if (weightOrError.IsError) return weightOrError.Errors;

        return new Package
        {
            Id = PackageId.CreateUnique(),
            Name = name,
            Weight = weightOrError.Value,
            Destination = destination,
            Status = PackageStatusEnum.Stored
        };
    }

    public void PackageAdded()
    {
        RaiseEvent(new PackageDomainEvent(Id.Value));
    }
}