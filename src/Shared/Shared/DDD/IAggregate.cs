namespace Shared.DDD;

public interface IAggregate<T>: IAggregate, IEntity<T> { }
public interface IAggregate
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents(); 
    void AddDomainEvent(IDomainEvent domainEvent);
}
