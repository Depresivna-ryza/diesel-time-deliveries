using Warehouse.Domain.Models.Package;
using Warehouse.Domain.Services;
using ErrorOr;

namespace Warehouse.Application;

public record ListPackagesQuery(int Page, int PageSize)
{ 
    public record Result(IEnumerable<Package> Packages);
    public record Package(Guid PackageId, string Name, decimal Weight, string Destination, string Status);
}

public class ListPackagesHandler(IQueryObject<Package> queryObject)
{
    public async Task<ErrorOr<ListPackagesQuery.Result>> Handle(ListPackagesQuery query)
    {
        var packages = await queryObject.Page(query.Page, query.PageSize).ExecuteAsync();
        
        return new ListPackagesQuery.Result(
            packages.Select(p => new ListPackagesQuery.Package(
                p.Id.Value,
                p.Name,
                p.Weight.Value,
                p.Destination,
                p.Status.ToString()
            ))
        );
    }
}
