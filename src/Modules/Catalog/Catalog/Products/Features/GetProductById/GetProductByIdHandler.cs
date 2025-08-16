
using Mapster;

namespace Catalog.Products.Features.GetProductById;
public record GetProductByIdResult(ProductDto Product);
public record GetProductByIdQuery(Guid ProductId):IQuery<GetProductByIdResult>;
internal class GetProductByIdHandler(CatalogDbContext _dbContext)
: IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        //retrieve data
        var product = await _dbContext.Products
        .FindAsync([query.ProductId], cancellationToken)
        ?? throw new Exception($"Product with Id:{query.ProductId} not found");
        var productdto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productdto);
    }
}