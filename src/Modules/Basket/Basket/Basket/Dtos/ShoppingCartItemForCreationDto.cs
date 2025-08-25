namespace Basket.Basket.Dtos;

public record ShoppingCartItemForCreationDto(Guid Id, Guid ShoppingCartId, Guid ProductId,
    int Quantity, string Color);