namespace Catalog.Contracts.Products.Features.GetProductById;
public record GetProductByIdResult(ProductDto Product);
public record GetProductByIdQuery(Guid ProductId) : IQuery<GetProductByIdResult>;
