using Delivery.Domain.Services;
using Delivery.Domain.Models;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Application.EndpointHandlers;

public record AddPlanCommand()
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

public class AddPlanHandler(IRepository<Plan> repository)
{
    public async Task<ErrorOr<AddPlanCommand.Result>> HandleAsync(AddPlanCommand command)
    {
        var plan = Plan.Create();

        if (plan.IsError)
            return plan.Errors;

        var addedPlan = await repository.InsertAsync(plan.Value);

        await repository.CommitAsync();
        return new AddPlanCommand.Result(
            addedPlan.Id.Value,
            addedPlan.VehicleId,
            addedPlan.CourierId,
            addedPlan.PackageIds,
            addedPlan.CurrentPackageIndex,
            addedPlan.Route, //TODO maybe to string?
            addedPlan.Status.ToString(),
            addedPlan?.StartedAt.ToString(), 
            addedPlan?.EndedAt.ToString()
        );
    }
}