using Delivery.Application.EndpointHandlers;
using ErrorOr;
using GoogleApi.Entities.Maps.Routes.Directions.Response;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Delivery.Presentation;

public record DeletePlanRequest(Guid PlanId)
{
    public record Response();
}

public class DeletePlanEndpoint
{
    [Tags("Delivery - Plan")]
    [WolverineDelete("/deletePlan")]
    public static async Task<IResult> DeletePlanAsync(DeletePlanRequest request, IMessageBus sender)
    {
        var command = new DeletePlanCommand(request.PlanId);

        var result = await sender.InvokeAsync<ErrorOr<DeletePlanCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new DeletePlanRequest.Response()
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}