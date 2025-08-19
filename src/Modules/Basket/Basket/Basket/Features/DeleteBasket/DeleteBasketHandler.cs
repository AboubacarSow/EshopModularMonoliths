
namespace Basket.Basket.Features.DeleteBasket;
public record DeleteBasketCommand(string UserName):ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccessed);

internal class DeleteBasketHandler(BasketDbContext _dbContext) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await _dbContext
            .ShoppingCarts.FindAsync([command.UserName], cancellationToken)
            ?? throw new BasketNotFoundException(command.UserName);
            
        _dbContext.ShoppingCarts.Remove(shoppingCart);

        await _dbContext.SaveChangesAsync();

        return new DeleteBasketResult(true);
        

    }
}