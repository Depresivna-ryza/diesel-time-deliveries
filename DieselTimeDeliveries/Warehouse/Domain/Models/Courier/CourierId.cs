using SharedKernel;

namespace Warehouse.Domain.Models.Courier;

public class CourierId  : ValueObject
{
    public Guid Value { get; private set; }
    
    private CourierId(Guid value)
    {
        Value = value;
    }

    public static CourierId CreateUnique() => new CourierId(Guid.NewGuid());
    public static CourierId Create(Guid value) => new CourierId(value);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}