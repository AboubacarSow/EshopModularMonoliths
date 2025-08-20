using System.Text.Json.Serialization;

namespace Basket.Basket.Models;

public class ShoppingCartItem: Entity<Guid>
{
    public Guid ShoppingCartId{get;private set;}=default!;
    public Guid ProductId{get;private set;}
    public string ProductName{ get; private set; }=default!;
    public string Color { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Quantity{get;internal set;} //(internal)because it's only modified within the basket assembly

    internal ShoppingCartItem(Guid shoppingCartId,Guid productId, string productName,
    string color,decimal price,int quantity)
    {
        ShoppingCartId=shoppingCartId;
        ProductId=productId;
        ProductId = productId;
        ProductName =productName;
        Color=color;
        Price=price;
        Quantity=quantity;
    }
    [JsonConstructor]
    public ShoppingCartItem(Guid id, Guid shoppingCartId, Guid productId, 
    decimal price, int quantity, string color, string productName)
    {
        Id = id;
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Price = price;
        Quantity = quantity;
        Color = color;
        ProductName = productName;
    }
}