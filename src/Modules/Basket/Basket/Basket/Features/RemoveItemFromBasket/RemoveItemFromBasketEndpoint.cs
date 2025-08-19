

namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketResponse(Guid Id);

public record RemoveItemFromBasketRequest(string UserName,Guid ProductId);
public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{username}/items/{productId}", async ( [FromRoute]string username,
        [FromRoute]Guid productId, ISender sender) =>
        {
            var command = new RemoveItemFromBasketCommand(username, productId);

            var result = await sender.Send(command);

            var response = result.Adapt<RemoveItemFromBasketResponse>();
           
            return Results.Ok(response);
        })
            .WithName("RemoveItemFromBasket")
            .Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Removing an item from basket")
            .WithDescription("Removing an item from basket");
    }
}
