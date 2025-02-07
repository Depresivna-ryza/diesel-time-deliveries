using Delivery.Domain.Models;
using ErrorOr;
using Delivery.Domain.Services;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Application.EndpointHandlers;

public record GetPlanQuery(Guid PlanId)
{
    public record Result(
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

public class GetPlanHandler(IQueryObject<Plan> queryObject)
{
    public async Task<ErrorOr<GetPlanQuery.Result>> Handle(GetPlanQuery query)
    {
        var plan = (await queryObject.Filter(p => p.Id == PlanId.Create(query.PlanId)).ExecuteAsync())
            .SingleOrDefault();

        if (plan is null)
            return Error.Validation($"Plan (id: {query.PlanId}) not found");

        return new GetPlanQuery.Result(
            plan.Id.Value,
            plan.VehicleId,
            plan.CourierId,
            plan.PackageIds,
            plan.CurrentPackageIndex,
            plan.Route, //TODO maybe to string?
            plan.Status.ToString(),
            plan?.StartedAt.ToString(), 
            plan?.EndedAt.ToString()
        );
    }
}