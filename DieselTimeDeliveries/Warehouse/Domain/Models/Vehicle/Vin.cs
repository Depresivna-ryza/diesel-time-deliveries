using System.Text.RegularExpressions;
using SharedKernel;
using ErrorOr;

namespace Warehouse.Domain.Models.Vehicle;

public class Vin : ValueObject
{
    public string Value { get; set; }

    private Vin(string value)
    {
        Value = value;
    }

    public static ErrorOr<Vin> Create(string value)
    {
        if ( !new Regex(@"^[A-Z]{2}[0-9]{3}[A-Z]{2}$").IsMatch(value))
        {
            return Error.Validation("Slovak Vin number is not valid");
        } 
        return new Vin(value);
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}