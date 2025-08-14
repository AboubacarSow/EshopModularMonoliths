namespace Catalog.Products.Events;
internal record ProductCreatedEvent(Product Product):IDomainEvent;
