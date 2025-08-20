using Basket.Data.Repositories;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart)
:ICommand<CreateBasketResult>;

internal record CreateBasketResult(Guid Id); 

public class CreateBasketValidator:AbstractValidator<CreateBasketCommand>
{
    public CreateBasketValidator()
    {
        RuleFor(s => s.ShoppingCart.UserName).NotEmpty().WithMessage("Username can not be empty or null");
    }
}

internal class CreateBasketHandler(IBasketRepository _basketRepository) : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var result = await _basketRepository.CreateBasket(command.ShoppingCart, cancellationToken);
        return new CreateBasketResult(result);
    }

    
}