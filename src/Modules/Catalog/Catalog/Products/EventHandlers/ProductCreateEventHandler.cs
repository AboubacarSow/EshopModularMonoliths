namespace Catalog.Products.EventHandlers;
internal class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> _logger) : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        await Task.CompletedTask;
    }
}