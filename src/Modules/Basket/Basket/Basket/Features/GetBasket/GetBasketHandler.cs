using Basket.Data.Repositories;
using Shared.Contracts.CQRS;

namespace Basket.Basket.Features.GetBasket;
internal record GetBasketQuery(string UserName):IQuery<GetBasketResult>;

internal record GetBasketResult(ShoppingCartDto ShoppingCart);


internal class GetBasketHandler(IBasketRepository _basketRepository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var bakset = await _basketRepository.GetBasket(query.UserName, false, cancellationToken);
        var result = bakset.Adapt<ShoppingCartDto>();
        return new GetBasketResult(result);
    }

}