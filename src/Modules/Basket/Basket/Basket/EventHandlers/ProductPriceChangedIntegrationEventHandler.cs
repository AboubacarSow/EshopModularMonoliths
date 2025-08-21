using Basket.Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Basket.Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler
(ILogger<ProductPriceChangedIntegrationEventHandler> logger,ISender sender) : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event Handling:{integration}",context.Message.GetType().Name);
        var result=await sender.Send(new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price));

        if(!result.IsSuccesseded)
        {
            logger
                .LogError("[Error]:an error occured while updating Price in basket with Product Id:{ProductId}",
                context.Message.ProductId.ToString());
        }

        logger.LogInformation("Price for Produt Id:{ProductId} updated in Basket",
            context.Message.ProductId.ToString());
 
    }
}