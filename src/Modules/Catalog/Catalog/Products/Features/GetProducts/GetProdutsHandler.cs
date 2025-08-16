using Mapster;

namespace Catalog.Products.Features.GetProducts;
public record GetProductsResult(IEnumerable<ProductDto> Products);
public record GetProductsQuery : IQuery<GetProductsResult>;

internal class GetProductsHandler(CatalogDbContext _dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //retrieving data
        var products = await _dbContext.Products
                                       .AsNoTracking()
                                       .OrderBy(p => p.Name)
                                       .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<IEnumerable<ProductDto>>();
        return new GetProductsResult(productDtos);
    }
}
