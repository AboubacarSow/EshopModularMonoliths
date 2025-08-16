namespace Catalog.Products.Features.CreateProduct;
internal record CreateProductResult(Guid Id);
internal record CreateProductCommand(ProductDto Product): ICommand<CreateProductResult>;

internal class CreateProductCommandHandler(CatalogDbContext _dbContext) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        //create new produt
        var product = CreateNewProduct(command.Product);
        //save to database
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        //return result
        return new CreateProductResult(product.Id);
    }

    private static Product CreateNewProduct(ProductDto product)
    {
        return Product.Create(
            Guid.NewGuid(),
            product.Name,
            product.Category,
            product.Description,
            product.ImageFile,
            product.Price
        );
    }
}