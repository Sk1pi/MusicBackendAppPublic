namespace MusicBackendApp.Application.Common.Exception;

public class CustomNotFoundException : System.Exception 
{
    public CustomNotFoundException() : base() {}
    
    public CustomNotFoundException(string name, object key) 
        : base($"Entity {name} with key {key.ToString()} was not found.") {}
}