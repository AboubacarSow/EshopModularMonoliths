namespace Shared.Contracts.CQRS;

public interface IQuery<out T> : IRequest<T> where T : notnull
{

}
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
 where TQuery : IQuery<TResponse>
 where TResponse : notnull
{

}