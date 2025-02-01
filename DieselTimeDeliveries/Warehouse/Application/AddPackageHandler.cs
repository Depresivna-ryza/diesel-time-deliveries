using Warehouse.Domain.Models.Package;
using Warehouse.Domain.Services;

namespace Warehouse.Application;
using ErrorOr;

public record AddPackageCommand(string Name, decimal Weight, string Destination)
{
    public record Result(AddedPackage Package);

    public record AddedPackage(Guid Id, string Name, decimal Weight, string Destination);
}

public class AddPackageHandler(IRepository<Package> _repository)
{
    public async Task<ErrorOr<AddPackageCommand.Result>> HandleAsync(AddPackageCommand command)
    {
        var goods = Package.Create(command.Name, command.Weight, command.Destination);

        if (goods.IsError)
            return goods.Errors;
        
        var addedGoods = await _repository.InsertAsync(goods.Value);
        addedGoods.PackageAdded();
        
        await _repository.CommitAsync();
        return new AddPackageCommand.Result(
            new(
                addedGoods.Id.Value,
                addedGoods.Name,
                addedGoods.Weight.Value,
                addedGoods.Destination
            )
        );
    }
}