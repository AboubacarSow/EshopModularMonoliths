using Shared.DDD;

namespace Ordering.Orders.Events;
internal record OrderCreatedEvent(Order Order):IDomainEvent;