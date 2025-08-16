namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductResult(bool IsSuccessed);
public record DeleteProductCommand(Guid ProductId) : ICommand<DeleteProductResult>;

internal class DeleteProductHandler(CatalogDbContext _dbContext) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        //retreive product
        var product = await _dbContext.Products
        .FindAsync([command.ProductId], cancellationToken)
        ?? throw new Exception($"Product with Id:{command.ProductId} not found");

        //remove product from database
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        //return result
        return new DeleteProductResult(true);
    }
}