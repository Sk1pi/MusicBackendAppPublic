namespace MusicBackendApp.Application.Common.Parameters;

public class PagedResult<T>
{
    public IReadOnlyList<T> Data { get; }
    public int TotalCount { get; }
    
    public PagedResult(IReadOnlyList<T> data, int totalCount)
    {
        Data = data;
        TotalCount = totalCount;
    }
}