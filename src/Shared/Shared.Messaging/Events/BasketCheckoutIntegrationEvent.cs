namespace Shared.Messaging.Events;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public string UserName { get; set; } = default!;
    public decimal TotalPrice { get; set; }
    public Guid CustomerId { get; set; }

    //Shopping Address and Billing Address
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string? EmailAddress { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;

    //Payment
    public string? CardName { get; } = default!;
    public string CardNumber { get; } = default!;
    public string Expiration { get; } = default!;
    public string Cvv { get; } = default!;
    public int PaymentMethod { get; } = default!;
}