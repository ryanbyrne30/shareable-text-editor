using BackendService.Gateway.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetCurrentUserDocuments;

public class GetCurrentUserDocumentsRequest
{
    [PageValidation]
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    [PageSizeValidation]
    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10;

    [DocumentSortByValidation]
    [FromQuery(Name = "sort_by")]
    public string SortBy { get; set; } = "updated_at";
    
    [SortDirectionValidation]
    [FromQuery(Name = "sort_direction")]
    public string SortDirection { get; set; } = "desc";
}