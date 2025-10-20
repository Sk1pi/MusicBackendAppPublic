namespace MusicBackendApp.Application.Response;

public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);

public record Envelope
{
    public object? Result { get; }
    
    public List<ResponseError> Errors { get; }
    
    public DateTime TimeGenerated { get; }
    
    public Envelope(object? result, IEnumerable<ResponseError> errors)
    {
        Result = result;
        Errors = errors.ToList();
        TimeGenerated = DateTime.UtcNow;
    }

    //Зручний статичний метод для створення об'єкта Envelope для успішної відповіді.
    public static Envelope Ok(object? result = null) =>
        new(result, []);

    
    public static Envelope Error(IEnumerable<ResponseError> errors) =>
        new(null, errors);
}