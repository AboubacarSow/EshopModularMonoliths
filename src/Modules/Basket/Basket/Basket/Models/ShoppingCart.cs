namespace Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    public string UserName { get; private set; } = default!;
    private List<ShoppingCartItem> _items = [];
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);

    public string GetBasketKeyByItemProductId(Guid productId)
    {
       var items=_items.FirstOrDefault(i => i.ProductId == productId);
       if(items==null)
            return string.Empty;
        return this.UserName;
    }
    public static ShoppingCart Create(Guid id, string userName)
    {
        //Ensure that username is not null or empty
        ArgumentException.ThrowIfNullOrEmpty(userName);
        var shoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = userName
        };
        return shoppingCart;
    }

    public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentException.ThrowIfNullOrEmpty(productId.ToString());
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem is not null) {
            existingItem.Quantity += quantity;
        } else {
            var item = new ShoppingCartItem(Id, productId, productName, color, price, quantity);
            _items.Add(item);
        }
    }
    public void RemoveItem(Guid productId)
    {
        var item= Items.FirstOrDefault(i=>i.ProductId==productId);
        if (item is not null) _items.Remove(item);
    }

}
