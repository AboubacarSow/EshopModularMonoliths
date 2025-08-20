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

    public async Task<int> SaveChangesAsync(string? userName,CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
