namespace Ordering.Orders.Features.DeleteOrder;

internal record DeleteOrderResquest(Guid OrderId);

internal record DeleteOrderResponse(bool Success);
internal class DeleteOrderEnpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{orderId:guid}", async (Guid orderId, ISender sender) =>
        {
            var command = new DeleteOrderCommand(orderId);
            var result = await sender.Send(command);
            return Results.Ok(new DeleteOrderResponse(result.Success));
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Deletes an order by its unique identifier.")
        .WithDescription("Deletes an existing order from the system using its unique identifier.");
    }
}   