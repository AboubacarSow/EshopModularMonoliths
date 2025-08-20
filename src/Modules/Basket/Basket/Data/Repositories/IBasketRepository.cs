namespace Basket.Data.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default);
    Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName,CancellationToken cancellationToken = default);
}
