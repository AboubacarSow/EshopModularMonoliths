
using System.Globalization;
using Basket.Data.Repositories;
using Catalog.Contracts.Products.Features.GetProductById;

namespace Basket.Basket.Features.AddItemIntoBasket;
public record AddItemIntoBasketCommand(string UserName,ShoppingCartItemDto ShoppingCartItemDto)
:ICommand<AddItemIntoBasketResult>;
public record AddItemIntoBasketResult(bool IsSuccessed,Guid Id);

public class AddItemIntoBasketValidator:AbstractValidator<AddItemIntoBasketCommand>{

    public AddItemIntoBasketValidator()
    {
        
        RuleFor(s=>s.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(s=>s.ShoppingCartItemDto.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(s=>s.ShoppingCartItemDto.Quantity).GreaterThan(0);
    }
}

internal class AddItemIntoBasketHandler(IBasketRepository _basketRepository,ISender sender,BasketDbContext dbContext)
: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
        //First of all, we retreive the shopping cart
        var shoppingCart = await _basketRepository.GetBasket(command.UserName, true, cancellationToken);

        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItemDto.ProductId),cancellationToken);

        shoppingCart.AddItem(command.ShoppingCartItemDto.ProductId,
            command.ShoppingCartItemDto.Quantity,
            command.ShoppingCartItemDto.Color,
            result.Product.Price,
            result.Product.Name);

        
        var resultValue= await _basketRepository.SaveChangesAsync(command.UserName,cancellationToken);
        if(resultValue==0)
            return new AddItemIntoBasketResult(false,shoppingCart.Id);

        return new AddItemIntoBasketResult(true,shoppingCart.Id);
    }
}