using Contracts.Queries;
using Warehouse.Domain.Services;
using ErrorOr;
using Warehouse.Domain.Models.Courier;
using Warehouse.Domain.Models.Package;

namespace Warehouse.Application.CommandHandlers;

public class GetPackageHandler(
    IQueryObject<Domain.Models.Package.Package> queryObject)
{
    public async Task<ErrorOr<GetPackageQuery.Result>> HandleAsync(GetPackageQuery query)
    {
        var package = (await queryObject.Filter(p => p.Id == PackageId.Create(query.PackageId)).ExecuteAsync())
            .SingleOrDefault();
        
        if (package is null)
        {
            return Error.Validation("No available couriers");
        }
        
        return new GetPackageQuery.Result(package.Weight.Value, package.Destination, package.Status.ToString());
    }
}