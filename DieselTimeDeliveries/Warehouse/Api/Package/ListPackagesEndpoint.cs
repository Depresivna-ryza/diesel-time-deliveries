using Microsoft.AspNetCore.Http;
using Warehouse.Application;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Api;

public record ListPackagesResponse(IEnumerable<ListPackagesResponse.Package> Packages)
{
    public record Package(Guid Id, string Name, decimal Weight, string Destination, string Status);
}

public class ListPackagesEndpoint
{
    [Tags("Warehouse - Package")]
    [WolverineGet("/packages")]
    public static async Task<IResult> ListPackagesAsync(int page, int pageSize, IMessageBus sender)
    {
        var query = new ListPackagesQuery(page, pageSize);

        var result = await sender.InvokeAsync<ErrorOr<ListPackagesQuery.Result>>(query);
        
        return result.Match(
            value => Results.Ok(
                new ListPackagesResponse(
                    value.Packages.Select(o => 
                        new ListPackagesResponse.Package(
                            o.PackageId, 
                            o.Name, 
                            o.Weight, 
                            o.Destination, 
                            o.Status)
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );

    }
}

