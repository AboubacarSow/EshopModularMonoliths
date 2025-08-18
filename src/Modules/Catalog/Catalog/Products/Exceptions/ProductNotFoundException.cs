namespace Catalog.Products.Exceptions;

public class ProductNotFoundException: NotFoundException
{
    public ProductNotFoundException(Guid Id):base("Product",Id){

    }
}