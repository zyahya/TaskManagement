namespace TaskManagement.Domain.Helpers;

public class QueryObject
{
    public string? Title { get; set; }
    public string? Status { get; set; }
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; } = false;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
