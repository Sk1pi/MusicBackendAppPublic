namespace MusicBackendApp.Application.Common.Exception;

public class CustomNotFoundException : System.Exception //Тут має бути свій кастомний валідатор(потім застосувати в cqrs)
{
    public CustomNotFoundException() : base() {}
    
    public CustomNotFoundException(string name, object key) 
        : base($"Entity {name} with key {key.ToString()} was not found.") {}
    // Додатковий конструктор, якщо потрібно передавати лише повідомлення
    
    //public CustomNotFoundException(string message) : base(message) { }
}