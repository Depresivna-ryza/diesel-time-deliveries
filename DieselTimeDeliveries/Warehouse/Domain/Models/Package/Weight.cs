using SharedKernel;
using ErrorOr;

namespace Warehouse.Domain.Models.Package;

public class Weight : ValueObject
{
    public decimal Value { get; set; }

    private Weight(decimal value)
    {
        Value = value;
    }

    public static ErrorOr<Weight> Create(decimal value)
    {
        if (value <= 0)
        {
            return Error.Validation("Weight cannot be negative");
        }

        return new Weight(value);
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}