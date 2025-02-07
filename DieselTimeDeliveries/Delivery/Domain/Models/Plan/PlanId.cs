using SharedKernel;

namespace Delivery.Domain.Models;

public class PlanId : ValueObject
{
    private PlanId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static PlanId CreateUnique()
    {
        return new PlanId(Guid.NewGuid());
    }

    public static PlanId Create(Guid value)
    {
        return new PlanId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}