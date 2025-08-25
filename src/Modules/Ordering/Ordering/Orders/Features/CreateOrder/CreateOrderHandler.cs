namespace Ordering.Orders.Features.CreateOrder;

public record CreateOrderCommand(OrderForCreationDto Order):ICommand<CreateOrderResult>;
public record CreateOrderResult(Guid OrderId);

public class CreateOrderValidator:AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Order).NotNull();
        RuleFor(x => x.Order.CustomerId).NotEmpty();
        RuleFor(x => x.Order.OrderName).NotEmpty().MaximumLength(100)
        .WithMessage("Order name must not exceed 100 characters.");
        RuleFor(x=> x.Order.OrderName).NotEmpty().WithMessage("Order name is required.");
        RuleFor(x => x.Order.ShippingAddress).NotNull();
        RuleFor(x => x.Order.BillingAddress).NotNull();
        RuleFor(x => x.Order.Payment).NotNull();
        RuleFor(x => x.Order.Items).NotEmpty();
    }
}



internal class CreateOrderHandler(OrderingDbContext _dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateOrder(command.Order);

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id);
    }

    private static Order CreateOrder(OrderForCreationDto order)
    {
        // Map OrderForCreationDto to Order entity
        var shippingAddress = Address.Of(
            order.ShippingAddress.FirstName,
            order.ShippingAddress.LastName,
            order.ShippingAddress.EmailAddress,
            order.ShippingAddress.AddressLine,
            order.ShippingAddress.Country,
            order.ShippingAddress.State,
            order.ShippingAddress.ZipCode);

        var billingAddress = Address.Of(
            order.BillingAddress.FirstName,
            order.BillingAddress.LastName,
            order.BillingAddress.EmailAddress,
            order.BillingAddress.AddressLine,
            order.BillingAddress.Country,
            order.BillingAddress.State,
            order.BillingAddress.ZipCode);

        var payment =  Payment.Of(
            order.Payment.CardName,
            order.Payment.CardNumber,
            order.Payment.Expiration,
            order.Payment.Cvv,
            order.Payment.PaymentMethod);

        var newOrder = Order.Create(id: Guid.NewGuid(),
                        customerId:order.CustomerId,
                        orderName:$"{order.OrderName}_{new Random().Next()}", 
                        shippingAddress:shippingAddress,
                        billingAddress:billingAddress
                        ,payment:payment);

        foreach (var item in order.Items)
        {
            newOrder.Add(productId:item.ProductId,
            quantity: item.Quantity,
            price: item.Price);
        }
        return newOrder;
    }
}