using Warehouse.Domain.Models.Vehicle;
using Warehouse.Domain.Services;

namespace Warehouse.Application;
using ErrorOr;

public record AddVehicleCommand(string Make, string Model, decimal WeightLimit, string Vin)
{
    public record Result(Guid VehicleId, string Make, string Model, decimal WeightLimit, string Vin, string Status);

}

public class AddVehicleHandler(IRepository<Vehicle> repository)
{
    public async Task<ErrorOr<AddVehicleCommand.Result>> HandleAsync(AddVehicleCommand command)
    {
        var goods = Vehicle.Create(command.Make, command.Model, command.WeightLimit, command.Vin);

        if (goods.IsError)
            return goods.Errors;
        
        var addedGoods = await repository.InsertAsync(goods.Value);
        // addedGoods.VehicleAdded();
        
        await repository.CommitAsync();
        return new AddVehicleCommand.Result(
            addedGoods.Id.Value,
            addedGoods.Make,
            addedGoods.Model,
            addedGoods.PackageWeightLimit.Value,
            addedGoods.Vin.Value,
            addedGoods.Status.ToString()
        );
    }
}