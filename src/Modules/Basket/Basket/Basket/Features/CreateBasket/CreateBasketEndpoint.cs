namespace Basket.Basket.Features.CreateBasket;
public record CreateBasketRequest(ShoppingCartDto ShoppingCartDto);

public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateBasketCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<CreateBasketResponse>();


            return Results.Created($"/basket/{response.Id}",response);
        })
        .WithName("CreateBasket")
        .Produces<CreateBasketResponse>(StatusCodes.Status200OK)
        .WithSummary("Create Basket")
        .WithDescription("Create Basket");
    }
}
