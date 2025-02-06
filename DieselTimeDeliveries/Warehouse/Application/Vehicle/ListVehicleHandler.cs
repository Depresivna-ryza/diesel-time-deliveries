using Warehouse.Domain.Models.Vehicle;
using Warehouse.Domain.Services;
using ErrorOr;

namespace Warehouse.Application;

public record ListVehiclesQuery(int Page, int PageSize)
{ 
    public record Result(IEnumerable<Vehicle> Vehicles);
    public record Vehicle(Guid VehicleId,string Make, string Model, decimal WeightLimit, string Vin, string Status);
}

public class ListVehicleHandler(IQueryObject<Vehicle> queryObject)
{
    public async Task<ErrorOr<ListVehiclesQuery.Result>> Handle(ListVehiclesQuery query)
    {
        var vehicles = await queryObject.Page(query.Page, query.PageSize).ExecuteAsync();
        
        return new ListVehiclesQuery.Result(
            vehicles.Select(p => new ListVehiclesQuery.Vehicle(
                p.Id.Value,
                p.Make,
                p.Model,
                p.PackageWeightLimit.Value,
                p.Vin.Value,
                p.Status.ToString()
            ))
        );
    }
}
