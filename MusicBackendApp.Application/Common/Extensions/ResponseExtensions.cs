using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicBackendApp.Application.Response;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Extensions;

public static class ResponseExtensions
{
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var responseError = new ResponseError(error.Code, error.Message, null);
        
        //ObjectResult: Це реалізація IActionResult в ASP.NET Core,
        //яка дозволяє повернути об'єкт (у даному випадку envelope) і
        //явно встановити HTTP-статус код.
        var envelope = Envelope.Error([responseError]);

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode // Встановлює HTTP-статус
        };
    }
    
    public static ActionResult ToValidationErrorResponse(this ValidationResult result)
    {
        if (result.IsValid) //Якщо валідний, то просто робимо return
            throw new InvalidOperationException("Validation result can not be succeed");
        
        var validationErrors = result.Errors;
        
        var responseErrors = from validationError in validationErrors 
            let errorMessage = validationError.ErrorMessage 
            let error = Error.Deserialize(errorMessage) 
            select new ResponseError(error.Code, error.Message, validationError.PropertyName);
            
        var envelope = Envelope.Error(responseErrors); //Формування response error
        
        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest //Статус код 400 має бути фіксованим 
        };
    }
}