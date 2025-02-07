namespace Contracts.Queries;

public record GetPackagesForDeliveryQuery(Guid VehicleId)
{
    public record Result(IEnumerable<Package> Packages);

    public record Package(Guid PackageId, string destination);
}