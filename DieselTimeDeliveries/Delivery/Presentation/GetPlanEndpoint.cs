using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record GetPlanResponse(
    Guid PlanId,
    Guid VehicleId,
    Guid CourierId,
    List<Guid> PackageIds,
    int CurrentPackageIndex,
    Route Route,
    string Status,
    string? StartedAt,
    string? EndedAt);

public class GetPlanEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverineGet("/plan/{id}")]
    public static async Task<IResult> GetPlanAsync(Guid id, IMessageBus sender)
    {
        var query = new GetPlanQuery(id);

        var result = await sender.InvokeAsync<ErrorOr<GetPlanQuery.Result>>(query);

        return result.Match(
            value => Results.Ok(
                new GetPlanResponse(
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