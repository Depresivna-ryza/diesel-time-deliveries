using Delivery.Domain.Models;
using ErrorOr;
using Delivery.Domain.Services;

namespace Delivery.Application.EndpointHandlers;

public record DeletePlanCommand(Guid PlanId)
{
    public record Result();
}

public class DeletePlanHandler(
    IRepository<Plan> repository,
    IQueryObject<Plan> queryObject)
{
    public async Task<ErrorOr<DeletePlanCommand.Result>> HandleAsync(DeletePlanCommand command)
    {
        var plan = (await queryObject.Filter(p => p.Id == PlanId.Create(command.PlanId)).ExecuteAsync())
            .SingleOrDefault();

        if (plan is null)
        {
            return Error.NotFound("Plan not found");
        }

        var planId = PlanId.Create(command.PlanId);

        await repository.RemoveAsync(planId);
        await repository.CommitAsync();

        return new DeletePlanCommand.Result();
    }
}