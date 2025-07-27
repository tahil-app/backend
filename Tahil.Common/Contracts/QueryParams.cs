namespace Tahil.Common.Contracts;

public class QueryParams
{
    public QueryParams()
    {
        Page = 1;
        PageSize = 10;
        Filters = new List<QueryFilterParams>();
    }

    public virtual IEnumerable<QueryFilterParams>? Filters { get; set; }
    public QuerySortParamsModel? Sort { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int OffSet => PageSize * (Page - 1);
}

public enum SortDirection { Asc, Desc }
public record QuerySortParamsModel
{
    public string? ColumnName { get; set; }
    public SortDirection Direction { get; set; }
}

public enum FilterOperators
{
    Equals,
    Contains,
    In
}
public record QueryFilterParams
{
    public string? ColumnName { get; set; }
    public object? ColumnValue { get; set; }
    public FilterOperators Operator { get; set; }
}