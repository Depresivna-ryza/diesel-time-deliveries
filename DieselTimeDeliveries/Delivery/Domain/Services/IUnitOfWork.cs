namespace Delivery.Domain.Services;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken);
}