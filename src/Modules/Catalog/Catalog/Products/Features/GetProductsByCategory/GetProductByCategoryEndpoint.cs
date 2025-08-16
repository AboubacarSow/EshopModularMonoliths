namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<ProductDto> Products);


internal class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(category));

            var response = result.Adapt<GetProductsByCategoryResponse>();
            return Results.Ok(response);
            
        }).WithName("GetProductByCategory")
        .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get One Product By Category")
        .WithDescription("Get One Product By Category");
    }
}
