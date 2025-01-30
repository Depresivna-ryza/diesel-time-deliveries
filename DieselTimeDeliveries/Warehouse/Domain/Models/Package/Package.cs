using Microsoft.AspNetCore.Http.HttpResults;
using SharedKernel;
using ErrorOr;

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

    public static ErrorOr<Package> Create(string name)
    {
        return new Package //example only
        {
            Id = PackageId.CreateUnique(),
            Name = name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Description = "hovno",
            Status = "test",
            Weight = 69,
            Destination = "Destination",
            CourierId = Guid.Parse("1dc985b2-11db-460a-9abf-06377d155038")
        };
    }
}