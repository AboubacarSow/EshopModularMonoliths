
using Basket.Data.Repositories;

namespace Basket.Basket.Features.DeleteBasket;
public record DeleteBasketCommand(string UserName):ICommand<DeleteBasketResult>;
public record DeleteBasketResult(bool IsSuccessed);

internal class DeleteBasketHandler(IBasketRepository _basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    { 
        await _basketRepository.DeleteBasket(command.UserName, cancellationToken);
        return new DeleteBasketResult(true);
    }
}