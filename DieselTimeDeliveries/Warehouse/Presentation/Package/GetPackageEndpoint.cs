using ErrorOr;
using Microsoft.AspNetCore.Http;
using Warehouse.Application.EndpointHandlers.Package;
using Wolverine;
using Wolverine.Http;

namespace Warehouse.Presentation.Package;

public record GetPackageResponse(Guid PackageId, string Name, decimal Weight, string Destination, string Status);

public class GetPackageEndpoint
{
    [Tags("Warehouse - Package")]
    [WolverineGet("/package/{id}")]
    public static async Task<IResult> GetPackageAsync(Guid id, IMessageBus sender)
    {
        var query = new GetPackageQuery(id);

        var result = await sender.InvokeAsync<ErrorOr<GetPackageQuery.Result>>(query);

        return result.Match(
            value => Results.Ok(
                new GetPackageResponse(
                    value.PackageId,
                    value.Name,
                    value.Weight,
                    value.Destination,
                    value.Status
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}