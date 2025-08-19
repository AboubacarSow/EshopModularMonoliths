
using Mapster;

namespace Basket.Basket.Features.GetBasket;
internal record GetBasketQuery(string UserName):IQuery<GetBasketResult>;

internal record GetBasketResult(ShoppingCartDto ShoppingCart);


internal record GetBasketHandler(BasketDbContext _dbContext) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var shoppingCarts = await _dbContext.ShoppingCarts
        .AsNoTracking()
        .Include(s=>s.Items)
        .SingleOrDefaultAsync(s=>s.UserName==query.UserName,
                            cancellationToken)
        ?? throw new BasketNotFoundException(query.UserName);
                    
        var dto= shoppingCarts.Adapt<ShoppingCartDto>();
        return new GetBasketResult(dto);
    }
}