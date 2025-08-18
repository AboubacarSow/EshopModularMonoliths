namespace Shared.Pagination;

public class PaginatedResult<T>(int pageNumber,int pageSize,long count,IEnumerable<T> data)
 where T :class
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize{get;}=pageSize;
    public long Count{get;}=count;
    public IEnumerable<T> Data { get; } = data;
   
}