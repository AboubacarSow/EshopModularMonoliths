namespace Ordering.Orders.Dtos;

public record OrderDto(Guid Id,
        Guid CustomerId,
        string OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        List<OrderItemDto> Items);
public record AddressDto(string FirstName, 
        string LastName,string EmailAddress,
        string AddressLine,string Country,
        string State,string ZipCode);

public record PaymentDto(string CardName, 
        string CardNumber,string Expiration, 
        string Cvv,int PaymentMethod);

public record OrderItemDto(Guid ProductId, int Quantity, decimal Price);
