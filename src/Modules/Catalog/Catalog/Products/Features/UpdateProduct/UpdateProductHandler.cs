namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductResult(bool IsSuccessed);

public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Product.Id)
        .NotEmpty().WithMessage("Product's Id is required");

        RuleFor(p => p.Product.Name)
        .NotEmpty().WithMessage("Name is required");
        
        RuleFor(p => p.Product.Price)
        .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductHandler(CatalogDbContext _dbContext) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        //retreiving the given product
        var product = await _dbContext.Products
        .FindAsync([command.Product.Id], cancellationToken)
        ?? throw new ProductNotFoundException(command.Product.Id);

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