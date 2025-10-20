using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.User.Commands.CreateUser;
using MusicBackendApp.Application.User.Queries.GetTracks;
using MusicBackendApp.Application.User.Queries.GetUserById;
using MusicBackendApp.Application.User.Queries.GetUserByUserName;
using MusicBackendApp.Domain.Entites.Enums.RolePermission;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Controllers;

[Route("api/Users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult> GetUsers(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetUserByIUserQuery {Name = name}, cancellationToken);
        return Ok(vm);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "ReadProfilePolicy")] 
    public async Task<ActionResult<UserIdVm>> GetUserById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var vm = await _mediator.Send(new GetUserByIdQuery {Id = id}, cancellationToken);
        return Ok(vm);
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);

        if (userId.IsFailure)
        {
            return BadRequest(userId.Error.Message);
        }
        
        var newUserId = userId.Value;
        
        return CreatedAtAction("GetUserById", new {Id = newUserId}, new { UserId = newUserId });
    }
    
    [HttpGet("me/favorites")]
    [Authorize(Policy = "ReadFavoritesPolicy")]
    public async Task<IActionResult> GetMyFavoriteTracks(CancellationToken cancellationToken)
    {
        var query = new GetMyFavoriteTracksQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    [HttpPut("{id}")] 
    [Authorize(Policy = "UpdateArtistPolicy")]
    public IActionResult UpdateArtist(UserId id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Function not implemented. Please create UpdateArtistCommand." });
    }
    
    [HttpDelete("{id}")] 
    [Authorize(Policy = "DeleteArtistPolicy")]
    public IActionResult DeleteArtist(UserId id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Function not implemented. Please create DeleteArtistCommand." });
    }
}