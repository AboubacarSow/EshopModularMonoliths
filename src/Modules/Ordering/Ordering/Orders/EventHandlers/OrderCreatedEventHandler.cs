using Microsoft.Extensions.Logging;

namespace Ordering.Orders.EventHandlers;

public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger) 
    : INotificationHandler<OrderCreatedEvent>
{
    public  async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handled: {Type}",notification.GetType().Name);
        await Task.CompletedTask;
    }
}
