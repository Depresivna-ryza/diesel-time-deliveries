using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record GetNextPlanDestinationRequest(Guid PlanId, bool CurrPackageDeliverySuccessul)
{
    public record Response(Guid PlanId, Guid? PackageId);
}

public class GetNextPlanDestinationEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverinePatch("/getNextPlanDestination")]
    public static async Task<IResult> AddPlanAsync(GetNextPlanDestinationRequest request, IMessageBus sender)
    {
        var command = new GetNextPlanDestinationQuery(
            request.PlanId, 
            request.CurrPackageDeliverySuccessul);

        var result = await sender.InvokeAsync<ErrorOr<GetNextPlanDestinationQuery.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new GetNextPlanDestinationRequest.Response(
                    value.PlanId,
                    value.PackageId
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}