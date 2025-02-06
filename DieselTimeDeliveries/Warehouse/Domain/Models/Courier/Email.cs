using SharedKernel;
using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Domain.Models.Courier;

public class Email : ValueObject
{
    public string Value { get; set; }

    private Email(string value)
    {
        Value = value;
    }

    public static ErrorOr<Email> Create(string value)
    {
        if ( ! new EmailAddressAttribute().IsValid(value))
        {
            return Error.Validation("Email is not valid");
        } 
        return new Email(value);
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}