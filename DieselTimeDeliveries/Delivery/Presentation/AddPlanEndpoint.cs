using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record AddPlanRequest()
{
    public record Response(
        Guid PlanId,
        Guid VehicleId,
        Guid CourierId,
        List<Guid> PackageIds,
        int CurrentPackageIndex,
        Route Route,
        string Status,
        string? StartedAt,
        string? EndedAt
    );
}

public class AddPlanEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverinePost("/addPlan")]
    public static async Task<IResult> AddPlanAsync(AddPlanRequest request, IMessageBus sender)
    {
        var command = new AddPlanCommand();

        var result = await sender.InvokeAsync<ErrorOr<AddPlanCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new AddPlanRequest.Response(
                    value.PlanId,
                    value.VehicleId,
                    value.CourierId,
                    value.PackageIds,
                    value.CurrentPackageIndex,
                    value.Route, //TODO maybe to string?
                    value.Status.ToString(),
                    value?.StartedAt.ToString(), 
                    value?.EndedAt.ToString()
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}