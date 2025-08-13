using MediatR;

namespace Shared.DDD;

public interface IDomainEvent:INotification
{
    Guid EventId => Guid.NewGuid();
    DateTime OccuredOn => DateTime.UtcNow;
    string EventType => GetType().AssemblyQualifiedName!;
}
