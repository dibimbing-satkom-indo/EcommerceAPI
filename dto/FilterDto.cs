public class FilterDto
{
    public string? Name { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public int CategoryID { get; set; }
    public string? SortBy { get; set; }
    public string? Sortorder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? CategoryName { get; set; }
    public bool? InStock { get; set; }

}

public class PagedResponse<T>
{
    public bool Success { get; set; }
    public string? Messages { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int? NextPage { get; set; }
    public int? PreviousPage { get; set; }
}