using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;
internal class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger,IBus bus)
 : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        //From here we are implementing what we call:"Integration Event"
        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId=notification.Product.Id,
            Name=notification.Product.Name,
            Price=notification.Product.Price,
            Description=notification.Product.Description,
            ImageFile=notification.Product.ImageFile,
            Category=notification.Product.Category,
        };
        await bus.Publish(integrationEvent,cancellationToken);
        await Task.CompletedTask;
    }
}