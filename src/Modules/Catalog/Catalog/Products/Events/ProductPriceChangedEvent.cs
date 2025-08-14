namespace Catalog.Products.Events;

internal record ProductPriceChangedEvent(Product Product):IDomainEvent;