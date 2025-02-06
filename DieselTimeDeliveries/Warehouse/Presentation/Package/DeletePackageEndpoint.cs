using Microsoft.AspNetCore.Http;
using Warehouse.Application.Package;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Presentation.Package;

public record DeletePackageRequest(Guid PackageId)
{
    public record Response();
}

public class DeletePackageEndpoint
{
    [Tags("Warehouse - Package")]
    [WolverineDelete("/deletePackage")]
    public static async Task<IResult> DeletePackagesAsync(DeletePackageRequest request, IMessageBus sender)
    {
        var command = new DeletePackageCommand(request.PackageId);

        var result = await sender.InvokeAsync<ErrorOr<DeletePackageCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new DeletePackageRequest.Response()
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}



