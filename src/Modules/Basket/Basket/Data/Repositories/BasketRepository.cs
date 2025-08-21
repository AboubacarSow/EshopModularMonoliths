using Basket.Basket.Features.UpdateItemPriceInBasket;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;

namespace Basket.Data.Repositories;

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
    public async Task<bool> UpdateItemPriceInBasket(Guid productId,decimal price,CancellationToken cancellationToken=default)
    {

        var shoppingCartItems = await _dbContext.ShoppingCartItems
           .Where(i => i.ProductId == productId).ToListAsync(cancellationToken);
        if (!shoppingCartItems.Any())
            return false;
        shoppingCartItems.ForEach(item =>
        {
            item.UpdatePrice(price);
        });
        return true;
    } 

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default, params string?[] userNames)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetBasketUserNamesByItemId(Guid ProductId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var baskets = !trackChanges ?
            await _dbContext.ShoppingCarts.AsNoTracking().ToListAsync(cancellationToken) :
            await _dbContext.ShoppingCarts.ToListAsync(cancellationToken);
        var keys = new List<string>(); 
        foreach(var basket in baskets)
        {
            var key = basket.GetBasketKeyByItemProductId(ProductId);
            if (!string.IsNullOrEmpty(key))
                keys.Add(key);
        }
        return keys;
    }
}
