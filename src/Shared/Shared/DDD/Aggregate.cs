namespace Shared.DDD;
public class Aggregate<T> : Entity<T>, IAggregate<T>
{
    private readonly List<IDomainEvent> _domainEvents=[];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public IDomainEvent[] ClearDomainEvents()
    {
        //We set events to a local variable
        IDomainEvent[] domainEvents = [.. _domainEvents];
        _domainEvents.Clear();
        return domainEvents;
    }
}
