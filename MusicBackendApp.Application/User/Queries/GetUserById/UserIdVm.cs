using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;

namespace MusicBackendApp.Application.User.Queries.GetUserById;

public class UserIdVm : IMapWith<Domain.Entites.User>
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Password { get; init; }
    public string? Email { get; init; }
    public decimal LikedTracks { get; init; }
    public int Subs { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entites.User, UserIdVm>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LikedTracks,
                opt => opt.MapFrom(src => src.LikedTracks))
            .ForMember(dest => dest.Subs,
                opt => opt.MapFrom(src => src.Subs));
    }
}