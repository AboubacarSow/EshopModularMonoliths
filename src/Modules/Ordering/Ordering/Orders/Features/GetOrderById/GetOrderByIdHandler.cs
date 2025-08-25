
namespace Ordering.Orders.Features.GetOrderById;    

public record GetOrderByIdQuery(Guid OrderId):IQuery<GetOrderByIdResult>;
public record GetOrderByIdResult(OrderDto Order);
public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required.");
    }
}

internal class GetOrderByIdHandler(OrderingDbContext _dbContext): IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.ShippingAddress)
            .Include(o => o.BillingAddress)
            .Include(o => o.Payment)
            .Include(o => o.Items)
            .SingleOrDefaultAsync(o => o.Id == query.OrderId, cancellationToken)
            ?? throw new OrderNotFoundException(query.OrderId);

        var orderDto = order.Adapt<OrderDto>();
        return new GetOrderByIdResult(orderDto);
    }

    
}