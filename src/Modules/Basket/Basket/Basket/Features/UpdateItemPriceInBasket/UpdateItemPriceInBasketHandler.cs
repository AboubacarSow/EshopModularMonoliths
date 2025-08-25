using Basket.Data.Repositories;

namespace Basket.Basket.Features.UpdateItemPriceInBasket;
public record UpdateItemPriceInBasketCommand(Guid ProductId,decimal Price):ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccesseded);
public class UpdateItemPriceInBasketValidator:AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketValidator()
    {
        RuleFor(p=>p.ProductId).NotEmpty().WithMessage("ProductId can not be empty or null");
        RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateItemPriceInBasketHandler(IBasketRepository basketRepository)
: ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        var updateResult = await basketRepository.UpdateItemPriceInBasket(command.ProductId, command.Price, cancellationToken);
        var userNames = await GetUserNames(command.ProductId, false, cancellationToken);
        var result=await basketRepository.SaveChangesAsync(cancellationToken,userNames);
        if (result == 0 && updateResult==false)
            return new UpdateItemPriceInBasketResult(false);
        return new UpdateItemPriceInBasketResult(true);
    }
    private async Task<string[]> GetUserNames(Guid productId,bool trackChanges,CancellationToken cancellationToken)
    {
        var keys = await basketRepository.GetBasketUserNamesByItemId(productId, trackChanges, cancellationToken);

        var userNames = new string[keys.Count];
        var count = 0;
        foreach (var key in keys)
        {
            userNames.SetValue(key, count);
            count++;
        }
        return userNames;
    }
}