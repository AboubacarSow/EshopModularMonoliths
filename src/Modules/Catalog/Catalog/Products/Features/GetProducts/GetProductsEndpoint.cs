
namespace Catalog.Products.Features.GetProducts;

public record GetProductsResponse(PaginatedResult<ProductDto> PaginatedResult);


internal class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters]PaginationParameters paginationParameters,ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery(paginationParameters));

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetAllProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Getting All Products")
        .WithDescription("Getting All Products");
    }
}