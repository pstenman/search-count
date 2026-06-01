using Microsoft.AspNetCore.Mvc;
using SearchCount.Api.Services;
using SearchCount.Api.Models;

namespace SearchCount.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly SearchService _searchService;

    public SearchController(SearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResponse>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { error = "Query parameter 'q' is required." });

        return Ok(await _searchService.SearchAsync(q));
    }
}