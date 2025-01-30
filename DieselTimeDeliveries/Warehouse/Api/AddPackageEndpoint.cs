using Microsoft.AspNetCore.Http;
using Warehouse.Application;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Api;

public record Request(string Name);

public class AddPackageEndpoint
{
    [Tags("Warehouse - Package")]
    [WolverinePost("/addPackage")]
    public static async Task<IResult> AddPackagesAsync(Request request, IMessageBus sender)
    {
        var command = new AddPackageCommand(request.Name);

        var result = await sender.InvokeAsync<ErrorOr<AddPackageCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new Response(
                    new(
                        value.Package.Id,
                        value.Package.Name
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}

public record Response(Response.Item Goods)
{
    public record Item(Guid Id, string Name);
}
