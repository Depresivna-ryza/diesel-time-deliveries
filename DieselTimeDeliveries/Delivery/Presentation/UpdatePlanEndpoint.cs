using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record UpdatePlanRequest(
    Guid PlanId, 
    Guid? VehicleId, 
    Guid? CourierId, 
    List<Guid>? PackageIds,
    string? Status)
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
        string? EndedAt);
}

public class UpdatePlanEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverinePatch("/updatePlan")]
    public static async Task<IResult> AddPlanAsync(UpdatePlanRequest request, IMessageBus sender)
    {
        var command = new UpdatePlanCommand(
            request.PlanId, 
            request.VehicleId,  
            request.CourierId, 
            request.PackageIds, 
            request.Status);

        var result = await sender.InvokeAsync<ErrorOr<UpdatePlanCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new UpdatePlanRequest.Response(
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