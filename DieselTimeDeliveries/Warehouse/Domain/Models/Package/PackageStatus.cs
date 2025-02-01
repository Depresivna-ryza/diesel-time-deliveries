using SharedKernel;

namespace Warehouse.Domain.Models.Package;

public class PackageStatus : ValueObject
{
    public PackageStatusEnum PackageStatusEnum { get; private set; }

    private PackageStatus(PackageStatusEnum packageStatusEnum)
    {
        PackageStatusEnum = packageStatusEnum;
    }
    
    public static PackageStatus Create(PackageStatusEnum packageStatusEnum) => new PackageStatus(packageStatusEnum);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PackageStatusEnum;
    }
}