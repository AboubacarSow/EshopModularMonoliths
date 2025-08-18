
namespace Catalog.Products.Features.GetProducts;
public record GetProductsResult(PaginatedResult<ProductDto> PaginatedResult);
public record GetProductsQuery(PaginationParameters PaginationParameters) : IQuery<GetProductsResult>;

internal class GetProductsHandler(CatalogDbContext _dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //retrieving data
        var pageNumber= query.PaginationParameters.PageNumber;
        var pageSize= query.PaginationParameters.PageSize;
        var totalCount = await _dbContext.Products.LongCountAsync(cancellationToken);


        var products = await _dbContext.Products
                                       .AsNoTracking()
                                       .OrderBy(p => p.Name)
                                       .Skip((pageNumber-1)*pageSize)
                                       .Take(pageSize)
                                       .ToListAsync(cancellationToken);

        var productDtos = products.Adapt<IEnumerable<ProductDto>>();
        return new GetProductsResult(new PaginatedResult<ProductDto>(pageNumber,pageSize,totalCount,productDtos));
    }
}
