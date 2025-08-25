namespace Ordering.Orders.Features.GetOrderById;
internal record GetOrderByIdRequest(Guid OrderId);

internal record GetOrderByIdResponse(OrderDto Order);

internal class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
        {
            var query = new GetOrderByIdQuery(orderId);
            var result = await sender.Send(query);
            return Results.Ok(new GetOrderByIdResponse(result.Order));
        })
        .WithName("GetOrderById")
        .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Gets an order by its ID.")
        .WithDescription("Retrieves the details of an order using its unique identifier.");
    }
}



