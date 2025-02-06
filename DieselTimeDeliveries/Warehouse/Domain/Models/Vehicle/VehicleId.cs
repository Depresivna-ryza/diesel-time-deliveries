using SharedKernel;

namespace Warehouse.Domain.Models.Vehicle;

public class VehicleId  : ValueObject
{
    public Guid Value { get; private set; }
    
    private VehicleId(Guid value)
    {
        Value = value;
    }

    public static VehicleId CreateUnique() => new VehicleId(Guid.NewGuid());
    public static VehicleId Create(Guid value) => new VehicleId(value);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}