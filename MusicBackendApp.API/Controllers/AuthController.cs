using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.User.Commands.CreateUser;
using MusicBackendApp.Application.User.Commands.LoginUser;

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")] 
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Message);
        }
        
        return Ok(new { UserId = result.Value });
    }

    [HttpPost("login")] 
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error.Message); 
        }
        
        return Ok(result.Value);
    }
}