namespace Catalog.Products.EventHandlers;
internal class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
 : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        await Task.CompletedTask;
    }
}