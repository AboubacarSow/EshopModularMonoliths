using System.Text.Json;

namespace Basket.Basket.Features.CheckoutBasket;

internal record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout):ICommand<CheckoutBasketResult>;

internal record CheckoutBasketResult(bool IsSuccessed);

internal class CheckoutBasketHandler(BasketDbContext dbContext) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {

        using var transaction=await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try {
            var basket = await dbContext.ShoppingCarts
            .Include(i => i.Items)
            .SingleOrDefaultAsync(u => u.UserName == request.BasketCheckout.UserName, cancellationToken)
            ?? throw new BasketNotFoundException(request.BasketCheckout.UserName);


            var eventMessage = request.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice=basket.TotalPrice;

            var outBoxMessage = new OutBoxMessage(
                id: Guid.NewGuid(),
                type: typeof(BasketCheckoutIntegrationEvent).AssemblyQualifiedName!,
                content: JsonSerializer.Serialize(eventMessage),
                occuredOn: DateTime.UtcNow);
            
            dbContext.OutBoxMessages.Add(outBoxMessage);

            dbContext.ShoppingCarts.Remove(basket);

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CheckoutBasketResult(false);



        }
        catch {
            await transaction.RollbackAsync(cancellationToken);
            return new CheckoutBasketResult(false);
        }
        ///////////////////// CHECKOUT BASKET WITHOUT OUTBOX
        //var basket =
        //    await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);

        //var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
        //eventMessage.TotalPrice = basket.TotalPrice;

        //await bus.Publish(eventMessage, cancellationToken);

        //await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

        //return new CheckoutBasketResult(true);
        ///////////////////// CHECKOUT BASKET WITHOUT OUTBOX

        throw new NotImplementedException();
    }
}
