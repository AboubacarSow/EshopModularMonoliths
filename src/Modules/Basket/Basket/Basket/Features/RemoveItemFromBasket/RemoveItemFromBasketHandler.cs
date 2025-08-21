
using Basket.Data.Repositories;

namespace Basket.Basket.Features.RemoveItemFromBasket;
public record RemoveItemFromBasketCommand(string UserName, Guid ProductId):ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(bool IsSuccessed,Guid Id);
public class RemoveItemFromBasketValidator:AbstractValidator<RemoveItemFromBasketCommand>
{

    public RemoveItemFromBasketValidator()
    {
        RuleFor(s=>s.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(s => s.ProductId).NotEmpty().WithMessage("ProductId is required");
    }
}

internal class RemoveItemFromBasketHandler(IBasketRepository _basketRepository)
: ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await _basketRepository.GetBasket(command.UserName, true, cancellationToken);
        shoppingCart.RemoveItem(command.ProductId);
        var result= await _basketRepository.SaveChangesAsync(command.UserName,cancellationToken);
        if (result == 0)
            return new RemoveItemFromBasketResult(false, shoppingCart.Id);
        return new RemoveItemFromBasketResult(true,shoppingCart.Id);
    }
}