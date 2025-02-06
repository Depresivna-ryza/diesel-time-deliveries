using SharedKernel;
using ErrorOr;

namespace Warehouse.Domain.Models.Courier;

public class Courier : AggregateRoot<CourierId>
{
    public string Name { get; private set; }
    
    public Email Email { get; private set; }
    
    public CourierStatusEnum Status { get; private set; }
    public static ErrorOr<Courier> Create(string name, string email)
    {
        var emailOrError = Email.Create(email);
        if (emailOrError.IsError)
        {
            return emailOrError.Errors;
        }

        return new Courier {
            Id = CourierId.CreateUnique(), 
            Name = name,
            Email = emailOrError.Value,
            Status = CourierStatusEnum.NotWorking,
        };
    }

    // public void CourierAdded() 
    // {
    //     RaiseEvent(new CourierDomainEvent(Id.Value));
    // }

}