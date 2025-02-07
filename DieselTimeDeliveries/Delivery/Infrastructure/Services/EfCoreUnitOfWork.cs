using RegistR.Attributes.Base;
using Delivery.Domain.Services;
using Delivery.Infrastructure.Persistence;

namespace Delivery.Infrastructure.Services;

[Register<IUnitOfWork>]
public class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly DeliveryDbContext _context;

    public EfCoreUnitOfWork(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}