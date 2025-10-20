using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.Common.Interfaces.Search;

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    
    [HttpGet("tracks")]
    public async Task<IActionResult> SearchTracks([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query cannot be empty.");
        }
        var results = await _searchService.SearchTrackAsync(query);
        return Ok(results);
    }
    
    [HttpGet("artists")]
    public async Task<IActionResult> SearchArtists([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query cannot be empty.");
        }
        var results = await _searchService.SearchArtistAsync(query);
        return Ok(results);
    }
}