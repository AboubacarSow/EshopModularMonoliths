using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;
internal record GetOrdersRequest(PaginationParameters RequestParameters);
internal record GetOrdersResponse(PaginatedResult<OrderDto> PaginatedResult);

internal class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters]PaginationParameters request, ISender sender) =>
        {
           
            var query = new GetOrdersQuery(new PaginationParameters(request.PageNumber, request.PageSize));
            var result = await sender.Send(query);
            return Results.Ok(new GetOrdersResponse(result.PaginatedResult));
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .WithSummary("Gets a paginated list of orders.")
        .WithDescription("Retrieves a list of orders with pagination support. You can specify the page number and page size as query parameters.");
    }
}