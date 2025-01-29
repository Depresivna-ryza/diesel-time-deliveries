using SharedKernel;

namespace Warehouse.Domain.Models.Package;

public class PackageId  : ValueObject
{
    public Guid Value { get; private set; }
    
    private PackageId(Guid value)
    {
        Value = value;
    }

    public static PackageId CreateUnique() => new PackageId(Guid.NewGuid());
    public static PackageId Create(Guid value) => new PackageId(value);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}