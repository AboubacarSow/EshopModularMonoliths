using Shared.Pagination;

namespace Ordering.Orders.Features.GetOrders;

public record GetOrdersQuery(PaginationParameters RequestParameters):IQuery<GetOrdersResult>;
public record GetOrdersResult(PaginatedResult<OrderDto> PaginatedResult);


internal class GetOrdersHandler(OrderingDbContext _dbContext): IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageNumber= query.RequestParameters.PageNumber;
        var pageSize= query.RequestParameters.PageSize;
        var totalOrders= await _dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.Payment)
            .Include(o => o.Items)
            .OrderBy(o => o.OrderName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var orderDtos = orders.Adapt<IEnumerable<OrderDto>>();
        return new GetOrdersResult(new PaginatedResult<OrderDto>(pageNumber,
        pageSize,
        totalOrders,
        orderDtos));
    }
    
}
