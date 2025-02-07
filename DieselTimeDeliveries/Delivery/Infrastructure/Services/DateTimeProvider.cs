using RegistR.Attributes.Base;
using Delivery.Domain.Services;

namespace Delivery.Infrastructure.Services;

[Register<IDateTimeProvider>]
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;

    public DateTime UtcNow => DateTime.UtcNow;
}