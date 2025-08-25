using MediatR;

namespace Ordering.Orders.EventHandlers;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    Task INotificationHandler<OrderCreatedEvent>.Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}