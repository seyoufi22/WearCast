namespace WearCast.Api.Common.Views;

public class PagingViewModel<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Records { get; set; }
    public int Pages { get; set; }
    public IEnumerable<T> Items { get; set; } = new List<T>();
}
