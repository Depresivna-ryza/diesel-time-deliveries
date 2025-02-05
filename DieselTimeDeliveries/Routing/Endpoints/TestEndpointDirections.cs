using Microsoft.AspNetCore.Http;
using Routing.Application;
using Wolverine;
using ErrorOr;
using Routing.Contracts.Commands;
using Wolverine.Http;

namespace Routing.Endpoints;

public record DirectionsRequest(string origin, string destination)
{
    public record Response(List<string> Direction) { }
}

public class TestEndpointDirections //only for testing purposes
{
    [Tags("Routing - Package")]
    [WolverinePost("/testdirection")]
    public static async Task<IResult> RoutePackagesAsync(DirectionsRequest destinatinRouteRequest, IMessageBus sender)
    {
        var command = new GetDirectionsCommand(destinatinRouteRequest.origin, destinatinRouteRequest.destination);

        var result = await sender.InvokeAsync<ErrorOr<GetDirectionsCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(value.StepInstructions),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}

