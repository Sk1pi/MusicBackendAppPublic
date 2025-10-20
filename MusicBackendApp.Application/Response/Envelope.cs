namespace MusicBackendApp.Application.Response;

//Це опис однієї конкретної помилки, яка сталася під час обробки запиту.
public record ResponseError(string? ErrorCode, string? ErrorMessage, string? InvalidField);//Опис помилки 
//InvalidField потрібен для того, що fluentvalidation для request, dto, який відправляє frontend, і ми хотіли побачити в нашому додатку, яке з цим свойств ствло неправильним 
//І сюди ми запишемо назву цього свойства 

public record Envelope
{
    public object? Result { get; }
    //буде містити успішні дані відповіді. Наприклад,
    //якщо запит GetArtistById був успішним,
    //сюди буде поміщено об'єкт ArtistByIdVm
    
    public List<ResponseError> Errors { get; }
    //Ця колекція буде заповнена,
    //якщо під час обробки запиту виникли одна або кілька помилок
    
    public DateTime TimeGenerated { get; }

    //Приймає результат і список помилок, і ініціалізує властивості.
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