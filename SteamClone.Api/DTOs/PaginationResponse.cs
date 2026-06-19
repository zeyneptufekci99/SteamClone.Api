namespace SteamClone.Api.DTOs;

public class PaginationResponse<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<T> Data { get; set; } = new();
}
