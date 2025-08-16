

namespace Catalog.Products.Features.UpdateProduct;
public record UpdateProductResult(bool IsSuccessed);

public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

internal class UpdateProductHandler(CatalogDbContext _dbContext) : ICommandHandler<UpdateProductCommand,UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        //retreiving the given product
        var product = await _dbContext.Products
        .FindAsync([command.Product.Id], cancellationToken)
        ?? throw new Exception($"Product with Id:{command.Product.Id} not found");

        UpdateProductWithNewValues(product, command.Product);

        //save to database
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }

    private static void UpdateProductWithNewValues(Product product, ProductDto newProduct)
    {
        product.Update(
            newProduct.Name,
            newProduct.Category,
            newProduct.Description,
            newProduct.ImageFile,
            newProduct.Price
        );
    }
}