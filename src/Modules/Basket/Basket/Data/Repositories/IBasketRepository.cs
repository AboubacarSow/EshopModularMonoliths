
namespace Basket.Data.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default);
    Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default,params string?[] userNames);
    Task<bool> UpdateItemPriceInBasket(Guid productId, decimal price, CancellationToken cancellationToken = default);
    Task<List<string>> GetBasketUserNamesByItemId(Guid ProductId, bool trackChanges, CancellationToken cancellationToken = default);
}
