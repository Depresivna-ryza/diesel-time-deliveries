using Warehouse.Domain.Models.Package;
using Warehouse.Domain.Services;

namespace Warehouse.Application;
using ErrorOr;

public record AddPackageCommand(string Name)
{
    public record Result(AddedPackage Package);

    public record AddedPackage(Guid Id, string Name);
}

public class AddPackageHandler(IRepository<Package> _repository)
{
    public async Task<ErrorOr<AddPackageCommand.Result>> HandleAsync(AddPackageCommand command)
    {
        var goods = Package.Create(command.Name);

        if (goods.IsError)
            return goods.Errors;
        
        var addedGoods = await _repository.InsertAsync(goods.Value);
        await _repository.CommitAsync();
        await Task.Delay(1000);
        
        return new AddPackageCommand.Result(
            new(
                addedGoods.Id.Value, 
                addedGoods.Name
            )
        );
    }
}