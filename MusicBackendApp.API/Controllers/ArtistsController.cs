using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.Artists.Commands.CreateArtist;
using MusicBackendApp.Application.Artists.Queries.GetArtistById;
using MusicBackendApp.Application.Artists.Queries.GetArtistByName;
using MusicBackendApp.Application.Artists.Queries.GetTopArtist;

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ArtistsController> _logger;

    // Конструктор тепер чистий і має лише одну залежність
    public ArtistsController(IMediator mediator,
        ILogger<ArtistsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ArtistListVm>> GetArtists(
        [FromQuery] string? name, 
        CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetArtistByNameQuery { Name = name }, cancellationToken);
        return Ok(vm);
    }
    
    [HttpGet("top")] 
    public async Task<ActionResult<ArtistListVm>> GetTopArtists(CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetTopArtistsQuery(), cancellationToken);
        return Ok(vm);
    }
    
    [HttpGet("{id:guid}")] 
    public async Task<ActionResult<ArtistByIdVm>> GetArtistById(
        [FromRoute] Guid id, // Приймаємо Guid
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("--- Отримано запит GetArtistById з ID: {ArtistId}", id);

        var query = new GetArtistByIdQuery { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        
        return Ok(result); 
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddArtist(
        [FromBody] CreateArtistCommand command,
        CancellationToken cancellationToken)
    {
        var artistId = await _mediator.Send(command, cancellationToken);
        
        if (artistId.IsFailure)
        {
            return BadRequest(artistId.Error.Message);
        }
        
        var newArtistId = artistId.Value;
        
        return CreatedAtAction("GetArtistById", new { id = newArtistId }, newArtistId);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateArtist(string id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, "Not Implemented");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteArtist(string id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, "Not Implemented");
    }
}