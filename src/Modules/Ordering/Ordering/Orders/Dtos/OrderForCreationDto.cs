namespace Ordering.Orders.Dtos;

public record OrderForCreationDto(Guid CustomerId,
        string OrderName,
        AddressDto ShippingAddress,
        AddressDto BillingAddress,
        PaymentDto Payment,
        List<OrderItemDto> Items);
