

namespace Ordering.Orders.Features.CreateOrder;
internal record CreateOrderRequest(OrderForCreationDto Order);
internal record CreateOrderResponse(Guid OrderId);

internal class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = new CreateOrderCommand(request.Order);
            var result = await sender.Send(command);
            return Results.Created($"/orders/{result.OrderId}", new CreateOrderResponse(result.OrderId));
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Creates a new order.")
        .WithDescription("Creates a new order with the provided details.");
    }
}