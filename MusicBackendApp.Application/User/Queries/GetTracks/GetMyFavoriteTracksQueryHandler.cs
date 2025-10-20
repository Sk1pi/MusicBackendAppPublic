using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Track.Queries.GetTrackByTitile;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.User.Queries.GetTracks;

public class GetMyFavoriteTracksQueryHandler
    : IRequestHandler<GetMyFavoriteTracksQuery, TrackListVm>
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public GetMyFavoriteTracksQueryHandler(
        IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor, 
        IMapper mapper)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<TrackListVm> Handle(GetMyFavoriteTracksQuery request, CancellationToken cancellationToken)
    {
        // 1. Отримуємо ID залогіненого користувача з токена
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdString, out var userIdGuid))
        {
            // Якщо користувач не авторизований, повертаємо порожній список
            return new TrackListVm { Tracks = new List<TrackLookupDto>() };
        }
        var userId = new UserId(userIdGuid);

        // 2. Викликаємо новий метод репозиторія
        var user = await _userRepository.GetUserWithFavoritesAsync(userId);

        if (user is null || user.FavoriteTracks is null)
        {
            return new TrackListVm { Tracks = new List<TrackLookupDto>() };
        }

        // 3. Мапимо колекцію треків на DTO
        var favoriteTracksDto = _mapper.Map<List<TrackLookupDto>>(user.FavoriteTracks);

        return new TrackListVm { Tracks = favoriteTracksDto };
    }
}