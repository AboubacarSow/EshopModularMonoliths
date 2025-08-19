
namespace Basket.Basket.Features.AddItemIntoBasket;
public record AddItemIntoBasketCommand(string UserName,ShoppingCartItemDto ShoppingCartItemDto)
:ICommand<AddItemIntoBasketResult>;
public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketValidator:AbstractValidator<AddItemIntoBasketCommand>{

    public AddItemIntoBasketValidator()
    {
        
        RuleFor(s=>s.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(s=>s.ShoppingCartItemDto.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(s=>s.ShoppingCartItemDto.Quantity).GreaterThan(0);
    }
}

internal class AddItemIntoBasketHandler(BasketDbContext _dbContext)
: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand query, CancellationToken cancellationToken)
    {
        //First of all, we retreive the shopping cart
        var shoppingCart= await _dbContext.ShoppingCarts
        .Include(s=>s.Items)
        .SingleOrDefaultAsync(s=>s.UserName==query.UserName,cancellationToken)
        ?? throw new BasketNotFoundException(query.UserName);

        shoppingCart.AddItem(query.ShoppingCartItemDto.ProductId, query.ShoppingCartItemDto.Quantity,
         query.ShoppingCartItemDto.Color, query.ShoppingCartItemDto.Price, query.ShoppingCartItemDto.ProductName);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}