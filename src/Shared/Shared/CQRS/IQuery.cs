using MediatR;
namespace Shared.CQRS;

public interface IQuery<out T> : IRequest<T> where T : notnull
{

}
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<T, TResponse>
 where TQuery : IQuery<TResponse>
 where TResponse : notnull
{

}