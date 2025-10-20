using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.Track.Commands.CreateTrack;
using MusicBackendApp.Application.Track.Queries.GetMusicById;
using MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TracksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TracksController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Отримує треки за назвою.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<TrackListVm>> GetTracks(
        [FromQuery] string? title,
        CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetTrackByTitileQuery { Title = title }, cancellationToken);
        return Ok(vm);
    }

    /// <summary>
    /// Отримує трек за ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrackIdVm>> GetTrackById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetTrackByIdQuery { Id = id }, cancellationToken);
        return Ok(vm);
    }

    /// <summary>
    /// Створює новий трек.
    /// </summary>
    [HttpPost("upload")]
    [Authorize] 
    public async Task<ActionResult> UploadTrack(
        [FromForm] CreateTrackCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error.Message); 
        }   
        
        var newTrackId = result.Value;
        return CreatedAtAction(nameof(GetTrackById), new { id = newTrackId }, newTrackId);
    }
    
    /// <summary>
    /// Оновлює існуючий трек (не реалізовано).
    /// </summary>
    [HttpPut("{id:guid}")]
    public IActionResult UpdateTrack(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, "Function not implemented.");
    }

    /// <summary>
    /// Видаляє трек (не реалізовано).
    /// </summary>
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTrack(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, "Function not implemented.");
    }
}