using ErrorOr;
using Warehouse.Domain.Services;

namespace Warehouse.Application.Package;

public record AddPackageCommand(string Name, decimal Weight, string Destination)
{
    public record Result(Guid PackageId, string Name, decimal Weight, string Destination, string Status);
}

public class AddPackageHandler(IRepository<Domain.Models.Package.Package> repository)
{
    public async Task<ErrorOr<AddPackageCommand.Result>> HandleAsync(AddPackageCommand command)
    {
        var goods = Domain.Models.Package.Package.Create(command.Name, command.Weight, command.Destination);

        if (goods.IsError)
            return goods.Errors;

        var addedGoods = await repository.InsertAsync(goods.Value);
        addedGoods.PackageAdded();

        await repository.CommitAsync();
        return new AddPackageCommand.Result(
            addedGoods.Id.Value,
            addedGoods.Name,
            addedGoods.Weight.Value,
            addedGoods.Destination,
            addedGoods.Status.ToString()
        );
    }
}