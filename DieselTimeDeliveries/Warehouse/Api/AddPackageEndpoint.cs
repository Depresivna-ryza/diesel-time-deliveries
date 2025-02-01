using Microsoft.AspNetCore.Http;
using Warehouse.Application;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Api;

public record AddPackageRequest(string Name, decimal Weight, string Destination);

public class AddPackageEndpoint
{
    [Tags("Warehouse - Package")]
    [WolverinePost("/addPackage")]
    public static async Task<IResult> AddPackagesAsync(AddPackageRequest request, IMessageBus sender)
    {
        var command = new AddPackageCommand(request.Name, request.Weight, request.Destination);

        var result = await sender.InvokeAsync<ErrorOr<AddPackageCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new AddPackageResponse(
                    value.PackageId,
                    value.Name,
                    value.Weight,
                    value.Destination,
                    value.Status.ToString()
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}

public record AddPackageResponse(Guid PackageId, string Name, decimal Weight, string Destination, string Status);