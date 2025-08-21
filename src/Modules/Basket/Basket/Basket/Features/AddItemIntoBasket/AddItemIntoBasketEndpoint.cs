
using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketResponse(bool IsSuccessed, Guid Id);
public record AddItemIntoBasketRequest(ShoppingCartItemDto ShoppingCartItemDto);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{UserName}/items", async([FromRoute]string UserName,[FromBody] AddItemIntoBasketRequest request, ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(UserName, request.ShoppingCartItemDto);

            var result = await sender.Send(command);

            var response = result.Adapt<AddItemIntoBasketResponse>();
  
            return Results.Created($"/basket/{response.Id}",response);
        }).WithName("AddItemIntoBasket")
        .Produces<AddItemIntoBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Addin a new item into basket")
        .WithDescription("Adding an item to basket");
    }
}