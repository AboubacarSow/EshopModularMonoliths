using Catalog.Products.Exceptions;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductResult(bool IsSuccessed);
public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(p => p.ProductId)
        .NotEmpty().WithMessage("Product's Id is required");
    }
}

internal class DeleteProductHandler(CatalogDbContext _dbContext) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        //retreive product
        var product = await _dbContext.Products
        .FindAsync([command.ProductId], cancellationToken)
        ?? throw new ProductNotFoundException(command.ProductId);

        //remove product from database
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        //return result
        return new DeleteProductResult(true);
    }
}