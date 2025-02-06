using Warehouse.Domain.Models.Courier;
using Warehouse.Domain.Services;

namespace Warehouse.Application;
using ErrorOr;

public record AddCourierCommand(string Name, string Email)
{
    public record Result(Guid CourierId, string Name, string Email, string Status);

}

public class AddCourierHandler(IRepository<Courier> repository)
{
    public async Task<ErrorOr<AddCourierCommand.Result>> HandleAsync(AddCourierCommand command)
    {
        var goods = Courier.Create(command.Name, command.Email);

        if (goods.IsError)
            return goods.Errors;
        
        var addedGoods = await repository.InsertAsync(goods.Value);
        
        // addedGoods.CourierAdded();
        
        await repository.CommitAsync();
        return new AddCourierCommand.Result(
            addedGoods.Id.Value,
            addedGoods.Name,
            addedGoods.Email.Value,
            addedGoods.Status.ToString()
        );
    }
}