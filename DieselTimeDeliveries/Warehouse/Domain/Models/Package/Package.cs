using SharedKernel;

namespace Warehouse.Domain.Models.Package;

public class Package : AggregateRoot<PackageId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Weight { get; private set; }
    public string Destination { get; private set; }
    public string Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid CourierId { get; private set; }
    public Courier Courier { get; private set; }
    
    
}