namespace Contracts.Queries;

public record GetPackageQuery(Guid PackageId)
{
    public record Result(decimal weight, string destination, string status);
}
