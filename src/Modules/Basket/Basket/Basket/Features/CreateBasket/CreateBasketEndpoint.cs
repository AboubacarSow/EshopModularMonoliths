using System.Security.Claims;

namespace Basket.Basket.Features.CreateBasket;
public record CreateBasketRequest(ShoppingCartForCreationDto ShoppingCart);

public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender, ClaimsPrincipal user) =>
        {

            var command = request.Adapt<CreateBasketCommand>();
            var updatedCommand = command.ShoppingCart with { UserName = user?.Identity?.Name! };
            var result = await sender.Send(command);

            var response = result.Adapt<CreateBasketResponse>();


            return Results.Created($"/basket/{response.Id}", response);
        })
        .WithName("CreateBasket")
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .WithSummary("Create Basket")
        .WithDescription("Create Basket")
        .RequireAuthorization();
    }
}
