namespace MusicBackendApp.Application.Common.Interfaces.Jwt;

public interface IJwtProvider
{
    string GenerateToken(Domain.Entites.User user);
}