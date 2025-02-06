using Microsoft.AspNetCore.Http;
using Warehouse.Application;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace Warehouse.Api;

public record ListCouriersResponse(IEnumerable<ListCouriersResponse.Courier> Couriers)
{
    public record Courier(Guid Id, string Name, string Email, string Status);
}

public class ListCouriersEndpoint
{
    [Tags("Warehouse - Courier")]
    [WolverineGet("/couriers")]
    public static async Task<IResult> ListCouriersAsync(int page, int pageSize, IMessageBus sender)
    {
        var query = new ListCouriersQuery(page, pageSize);

        var result = await sender.InvokeAsync<ErrorOr<ListCouriersQuery.Result>>(query);
        
        return result.Match(
            value => Results.Ok(
                new ListCouriersResponse(
                    value.Couriers.Select(o => 
                        new ListCouriersResponse.Courier(
                            o.CourierId, 
                            o.Name, 
                            o.Email,
                            o.Status)
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );

    }
}