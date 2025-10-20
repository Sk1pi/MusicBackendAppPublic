using Microsoft.AspNetCore.Mvc;

namespace MusicBackendApp.Controllers;

[ApiController]
[Route("api/[controller]")] 
public class MockStudentVerificationController : ControllerBase
{
    [HttpGet("verify")] 
    public IActionResult VerifyStudentCard([FromQuery] string cardId)
    {
        if (string.IsNullOrWhiteSpace(cardId))
        {
            return BadRequest(new { isValid = false, message = "Card ID cannot be empty." });
        }
        
        if (cardId.StartsWith("STUDENT")) 
        {
            return Ok(new { isValid = true, message = "Student card is valid." });
        }
        else 
        {
            return Ok(new { isValid = false, message = "Student card is not valid." });
        }
    }
}