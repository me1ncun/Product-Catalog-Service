namespace ProductCatalog.Application.DTOs;

public class GetProductsDto
{
    public string? SearchItem { get; set; }
    public string? SortColumn { get; set; } 
    public string? SortOrder { get; set; } 
    public int Page { get; set; }
    public int PageSize { get; set; }

    public GetProductsDto(
        string? searchTerm, 
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize)
    {
        SearchItem = searchTerm;
        SortColumn = sortColumn;
        SortOrder = sortOrder;
        Page = page;
        PageSize = pageSize;
        
    }
    public GetProductsDto(){}
}