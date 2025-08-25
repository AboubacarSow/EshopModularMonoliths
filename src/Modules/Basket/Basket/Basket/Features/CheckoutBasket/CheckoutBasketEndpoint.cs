

namespace Basket.Basket.Features.CheckoutBasket;


internal record CheckoutBasketRequest(BasketCheckoutDto BasketCheckout)
:ICommand<CheckoutBasketResponse>;

internal record CheckoutBasketResponse(bool IsSuccessed);
internal class CheckoutBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            var command = new CheckoutBasketCommand(request.BasketCheckout);
            var result = await sender.Send(command);
            return Results.Ok(new CheckoutBasketResponse(result.IsSuccessed));
        })
        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checks out the current user's basket.")
        .WithDescription("Checks out the currdnt user's basket and initiates the order process.")
        .RequireAuthorization();
    }
}