using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record ListPlansResponse(IEnumerable<ListPlansResponse.Plan> Plans)
{
    public record Plan(
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

public class ListPlansEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverineGet("/plans")]
    public static async Task<IResult> ListPlansAsync(int page, int pageSize, IMessageBus sender)
    {
        var query = new ListPlansQuery(page, pageSize);

        var result = await sender.InvokeAsync<ErrorOr<ListPlansQuery.Result>>(query);

        return result.Match(
            value => Results.Ok(
                new ListPlansResponse(
                    value.Plans.Select(plan =>
                        new ListPlansResponse.Plan(
                            plan.PlanId,
                            plan.VehicleId,
                            plan.CourierId,
                            plan.PackageIds,
                            plan.CurrentPackageIndex,
                            plan.Route, //TODO maybe to string?
                            plan.Status.ToString(),
                            plan?.StartedAt.ToString(), 
                            plan?.EndedAt.ToString())
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}