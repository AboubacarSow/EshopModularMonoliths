namespace Catalog.Products.Features.CreateProduct;
internal record CreateProductResult(Guid Id);
public  record CreateProductCommand(ProductDto Product): ICommand<CreateProductResult>;

public  class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public  CreateProductValidator()
    {
        RuleFor(p => p.Product.Name)
        .NotEmpty().WithMessage("Name is required");

        RuleFor(p => p.Product.Category)
        .NotEmpty().WithMessage("Category field is required");

        RuleFor(p => p.Product.ImageFile)
        .NotEmpty().WithMessage("Image is required");

        RuleFor(p => p.Product.Price)
        .GreaterThan(0).WithMessage("Price must be greater than 0");

    }
}
internal class CreateProductCommandHandler(CatalogDbContext _dbContext) 
: ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        //Implementig cross-cutting concerns:The code below need to be refactored by implementing the pipeline behaviors inside our Shared project
        //This is a matter of repeating this code in every handler which is not following the DRY pattern
        //1. validation concern
        //var result = await validator.ValidateAsync(command, cancellationToken);
        //var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        //if (errors.Count != 0)
        //   throw new ValidationException(errors.FirstOrDefault());
        //2. Perfoming logging
        //logger.LogInformation("CreateProductHandler.Handle called with {@command}", command);

        //Actuel logic
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