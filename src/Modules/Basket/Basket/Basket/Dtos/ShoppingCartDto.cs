namespace Basket.Basket.Dtos;
public record ShoppingCartDto(Guid Id, string UserName,List<ShoppingCartItemDto> Items);

public record ShoppingCartItemDto(Guid Id,Guid ShoppingCartId,Guid ProductId,
    int Quantity,string Color,decimal Price,string ProductName);

public record ShoppingCartItemForCreationDto(Guid Id, Guid ShoppingCartId, Guid ProductId,
    int Quantity, string Color);