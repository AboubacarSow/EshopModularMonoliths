namespace Catalog.Products.Exceptions;

public class ProductNotFoundException(Guid Id) : NotFoundException("Product",Id)
{
}