using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Basket.Data.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default);
    Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(string? userName,CancellationToken cancellationToken = default);
}
internal class BasketRepository(BasketDbContext _dbContext) : IBasketRepository
{
    public async Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default)
    {
        var shoppingCart = CreateNewShoppingCart(basket);
        _dbContext.ShoppingCarts.Add(shoppingCart);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return shoppingCart.Id;
    }
    private static ShoppingCart CreateNewShoppingCart(ShoppingCartDto shoppingCart)
    {
        var basket = ShoppingCart.Create(shoppingCart.Id, shoppingCart.UserName);
        shoppingCart.Items.ForEach(item =>
        basket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName));
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await GetBasket(userName, false, cancellationToken);
        _dbContext.ShoppingCarts.Remove(shoppingCart);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var shoppingCart = !trackChanges ?
         await _dbContext.ShoppingCarts.AsNoTracking().Include(s => s.Items).SingleOrDefaultAsync(s => s.UserName == userName, cancellationToken) :
         await _dbContext.ShoppingCarts.Include(s => s.Items).SingleOrDefaultAsync(s => s.UserName == userName, cancellationToken)
        ;
        return shoppingCart ?? throw new BasketNotFoundException(userName);
    }

    public async Task<int> SaveChangesAsync(string? userName,CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal class CachedBasketRepository(IBasketRepository basketRepository,IDistributedCache cache) 
: IBasketRepository
{
    public async Task<Guid> CreateBasket(ShoppingCartDto basket, CancellationToken cancellationToken = default)
    {
        var result = await basketRepository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
        return result;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await basketRepository.DeleteBasket(userName,cancellationToken);
        await cache.RemoveAsync(userName,cancellationToken);
        return true;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (trackChanges)
            return await basketRepository.GetBasket(userName, true, cancellationToken);

        //check if the data is in the cache
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        //if not
        var basket = await basketRepository.GetBasket(userName, trackChanges, cancellationToken);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<int> SaveChangesAsync(string? userName, CancellationToken cancellationToken = default)
    {
        var result=await basketRepository.SaveChangesAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(userName)) await cache.RemoveAsync(userName, cancellationToken);

        return result;
    }
}