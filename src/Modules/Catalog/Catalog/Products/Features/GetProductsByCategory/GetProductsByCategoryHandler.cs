namespace Catalog.Products.Features.GetProductsByCategory;
public record GetProductsByCategoryResult(IEnumerable<ProductDto> Products);
public record GetProductsByCategoryQuery(string Category):IQuery<GetProductsByCategoryResult>;

internal class GetProductsByCategoryHandler(CatalogDbContext _dbContext)
: IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {

        //retrieve data
        var products = await _dbContext.Products
                                    .AsNoTracking()
                                    .Where(p => p.Category.Contains(query.Category))
                                    .OrderBy(p => p.Name)
                                    .ToListAsync(cancellationToken);
        var productDtos = products.Adapt<IEnumerable<ProductDto>>();
        return new GetProductsByCategoryResult(productDtos);
    }
}