
using Catalog.Contracts.Products.Features.GetProductById;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

//public record GetProductByIdRequest(Guid Id);

internal class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));


            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        }).WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get One Product By Id")
        .WithDescription("Get One Product By Id");
    }
}