using Delivery.Domain.Models;
using ErrorOr;
using Delivery.Domain.Services;
using GoogleApi.Entities.Maps.Routes.Directions.Response;

namespace Delivery.Application.EndpointHandlers;

public record GetNextPlanDestinationQuery(Guid PlanId, bool CurrPackageDeliverySuccessul)
{
    public record Result(Guid PlanId, Guid? PackageId);
}

public class GetNextPlanDestinationHandler(IQueryObject<Plan> queryObject)
{
    public async Task<ErrorOr<GetNextPlanDestinationQuery.Result>> Handle(GetNextPlanDestinationQuery query)
    {
        var plan = (await queryObject.Filter(p => p.Id == PlanId.Create(query.PlanId)).ExecuteAsync())
            .SingleOrDefault();

        if (plan is null)
            return Error.Validation($"Plan (id: {query.PlanId}) not found");
        
        var nextPackageId = plan.ProceedToNextPackage(query.CurrPackageDeliverySuccessul);

        return new GetNextPlanDestinationQuery.Result(
            plan.Id.Value,
            nextPackageId
        );
    }
}