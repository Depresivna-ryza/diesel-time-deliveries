using SharedKernel;

namespace Warehouse.Domain.Models.Vehicle;

public class VehicleStatus : ValueObject
{
    public VehicleStatusEnum VehicleStatusEnum { get; private set; }

    private VehicleStatus(VehicleStatusEnum vehicleStatusEnum)
    {
        VehicleStatusEnum = vehicleStatusEnum;
    }
    
    public static VehicleStatus Create(VehicleStatusEnum vehicleStatusEnum) => new VehicleStatus(vehicleStatusEnum);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return VehicleStatusEnum;
    }
}