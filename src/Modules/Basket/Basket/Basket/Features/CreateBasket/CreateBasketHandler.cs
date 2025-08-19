namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCartDto)
:ICommand<CreateBasketResult>;

internal record CreateBasketResult(Guid Id); 

public class CreateBasketValidator:AbstractValidator<CreateBasketCommand>
{
    public CreateBasketValidator()
    {
        RuleFor(s => s.ShoppingCartDto.UserName).NotEmpty().WithMessage("Username can not be empty or null");
    }
}

internal class CreateBasketHandler(BasketDbContext _dbContext) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = CreateNewShoppingCart(command.ShoppingCartDto);

        _dbContext.ShoppingCarts.Add(shoppingCart);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CreateBasketResult(shoppingCart.Id);
    }

    private static ShoppingCart CreateNewShoppingCart(ShoppingCartDto shoppingCartDto)
    {
        var shoppingCart = ShoppingCart.Create(shoppingCartDto.Id, shoppingCartDto.UserName);
        shoppingCartDto.Items.ForEach(item =>
        shoppingCart.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName));
        return shoppingCart;
    }
}