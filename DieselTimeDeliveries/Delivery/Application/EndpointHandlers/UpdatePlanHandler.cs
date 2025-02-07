using Delivery.Domain.Models;
using ErrorOr;
using Delivery.Domain.Services;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Application.EndpointHandlers;

public record UpdatePlanCommand(
    Guid PlanId, 
    Guid? VehicleId, 
    Guid? CourierId, 
    List<Guid>? PackageIds,
    string? Status)
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
        string? EndedAt);
}

public class UpdatePlanHandler(IRepository<Plan> repository, IQueryObject<Plan> queryObject)
{
    public async Task<ErrorOr<UpdatePlanCommand.Result>> HandleAsync(UpdatePlanCommand command)
    {
        var plan = 
            (await queryObject.Filter(p => 
                p.Id == PlanId.Create(command.PlanId)).ExecuteAsync())
            .SingleOrDefault();
        
        if (plan is null) 
            return Error.Validation("Plan not found");
        
        var updateResult = plan.Update(
            command.VehicleId,  
            command.CourierId, 
            command.PackageIds,
            command.Status);
        
        if (updateResult.IsError) 
            return updateResult.Errors;
        
        var updatedPlan = plan;

        repository.Update(updatedPlan);
        await repository.CommitAsync();
        
        return new UpdatePlanCommand.Result(
            updatedPlan.Id.Value,
            updatedPlan.VehicleId,
            updatedPlan.CourierId,
            updatedPlan.PackageIds,
            updatedPlan.CurrentPackageIndex,
            updatedPlan.Route, //TODO maybe to string?
            updatedPlan.Status.ToString(),
            updatedPlan?.StartedAt.ToString(), 
            updatedPlan?.EndedAt.ToString()
        );
    }
}