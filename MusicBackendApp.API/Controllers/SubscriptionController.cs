using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.User.Commands.AssignFamilySubscription;
using MusicBackendApp.Application.User.Commands.PurchaseSubscription;
using MusicBackendApp.Contracts;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly ISender _mediator; 

    public SubscriptionController(ISender mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Купує нову підписку для користувача.
    /// </summary>
    /// <param name="request">Дані для покупки підписки.</param>
    /// <returns>Результат операції.</returns>
    [HttpPost("purchase")] // Маршрут: POST /api/subscription/purchase
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PurchaseSubscription([FromBody] PurchaseSubscriptionRequest request)
    {
        var command = new PurchaseSubscriptionCommand
        {
            UserId = request.UserId,
            SubscriptionType = request.SubscriptionType,
            PaymentType = request.PaymentType,
            StudentCardId = request.StudentCardId.Value
        };
        
        Result<Unit, Error> result = await _mediator.Send(command);
        
        if (result.IsSuccess)
        {
            return Ok(new { Message = "Subscription purchased successfully." });
        }
        else
        {
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }
    }
    
    /// <summary>
    /// Призначає головного користувача для сімейної підписки.
    /// </summary>
    /// <param name="subscriptionId">ID сімейної підписки.</param>
    /// <param name="request">Дані для призначення адміністратора.</param>
    /// <returns>Результат операції.</returns>
    [HttpPut("{subscriptionId}/assign-admin")] // PUT /api/subscription/{subscriptionId}/assign-admin
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignFamilySubscriptionAdmin(Guid subscriptionId, [FromBody] AssignFamilySubscriptionAdminRequest request)
    {
        var command = new AssignFamilySubscriptionAdminCommand
        {
            SubscriptionId = subscriptionId,
            UserId = request.UserId 
        };

        Result<Unit, Error> result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "Family subscription admin assigned successfully." });
        }
        else
        {
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }
    }
    
}