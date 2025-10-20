namespace MusicBackendApp.Infrastructure.Configurations.Search;

public class ElasticSettings
{
    public string Url { get; set; } 
    public string DefaultIndex { get; set; } 
    public string UserName { get; set; } 
    public string Password { get; set; } 
}