using SharedKernel;

namespace Warehouse.Domain.Models.Courier;

public class CourierStatus : ValueObject
{
    public CourierStatusEnum CourierStatusEnum { get; private set; }

    private CourierStatus(CourierStatusEnum courierStatusEnum)
    {
        CourierStatusEnum = courierStatusEnum;
    }
    
    public static CourierStatus Create(CourierStatusEnum courierStatusEnum) => new CourierStatus(courierStatusEnum);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CourierStatusEnum;
    }
}