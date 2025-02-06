using Microsoft.AspNetCore.Http;
using Warehouse.Application.Vehicle;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Presentation.Vehicle;

public record DeleteVehicleRequest(Guid VehicleId)
{
    public record Response();
}

public class DeleteVehicleEndpoint
{
    [Tags("Warehouse - Vehicle")]
    [WolverineDelete("/deleteVehicle")]
    public static async Task<IResult> DeleteVehiclesAsync(DeleteVehicleRequest request, IMessageBus sender)
    {
        var command = new DeleteVehicleCommand(request.VehicleId);

        var result = await sender.InvokeAsync<ErrorOr<DeleteVehicleCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new DeleteVehicleRequest.Response()
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}