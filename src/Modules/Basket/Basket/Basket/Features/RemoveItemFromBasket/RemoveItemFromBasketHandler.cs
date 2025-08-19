
namespace Basket.Basket.Features.RemoveItemFromBasket;
public record RemoveItemFromBasketCommand(string UserName, Guid ProductId):ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);
public class RemoveItemFromBasketValidator:AbstractValidator<RemoveItemFromBasketCommand>
{

    public RemoveItemFromBasketValidator()
    {
        RuleFor(s=>s.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}

internal class RemoveItemFromBasketHandler(BasketDbContext _dbContext)
: ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart= await _dbContext.ShoppingCarts
        .Include(i=>i.Items)
        .SingleOrDefaultAsync(s=>s.UserName==command.UserName,cancellationToken)
        ?? throw new BasketNotFoundException(command.UserName);

        shoppingCart.RemoveItem(command.ProductId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}