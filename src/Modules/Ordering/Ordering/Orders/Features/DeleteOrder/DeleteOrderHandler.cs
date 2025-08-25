using Ordering.Orders.Exceptions;


namespace Ordering.Orders.Features.DeleteOrder;
public record DeleteOrderCommand(Guid OrderId):ICommand<DeleteOrderResut>;
public record DeleteOrderResut(bool Success);
public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required.");
    }
}

internal class DeleteOrderHandler(OrderingDbContext _dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResut>
{
    public async Task<DeleteOrderResut> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FindAsync([command.OrderId], cancellationToken)
        ?? throw new OrderNotFoundException(command.OrderId);

        _dbContext.Orders.Remove(order);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResut(result > 0);
    }
}
