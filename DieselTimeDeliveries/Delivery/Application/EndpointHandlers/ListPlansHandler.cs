using Delivery.Domain.Models;
using ErrorOr;
using Delivery.Domain.Services;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Application.EndpointHandlers;

public record ListPlansQuery(int Page, int PageSize)
{
    public record Result(IEnumerable<Plan> Plans);

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

public class ListPlansHandler(IQueryObject<Plan> queryObject)
{
    public async Task<ErrorOr<ListPlansQuery.Result>> Handle(ListPlansQuery query)
    {
        var couriers = await queryObject.Page(query.Page, query.PageSize).ExecuteAsync();

        return new ListPlansQuery.Result(
            couriers.Select(plan => new ListPlansQuery.Plan(
                plan.Id.Value,
                plan.VehicleId,
                plan.CourierId,
                plan.PackageIds,
                plan.CurrentPackageIndex,
                plan.Route, //TODO maybe to string?
                plan.Status.ToString(),
                plan?.StartedAt.ToString(), 
                plan?.EndedAt.ToString()
            ))
        );
    }
}