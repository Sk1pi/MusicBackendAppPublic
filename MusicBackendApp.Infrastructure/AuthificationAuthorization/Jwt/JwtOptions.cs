namespace MusicBackendApp.Infrastructure.AuthificationAuthorization.Jwt;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    
    public int ExpiresHours { get; set; } 
}