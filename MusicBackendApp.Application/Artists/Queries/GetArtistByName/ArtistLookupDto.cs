using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;
using MusicBackendApp.Domain.Entites;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistByName;

public class ArtistLookupDto : IMapWith<Artist>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

   public void Mapping(Profile profile)
    {
        profile.CreateMap<Artist, ArtistLookupDto>()
            .ForMember(artistDto => artistDto.Id,
                opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(artistDto => artistDto.Name,
                opt => opt.MapFrom(src => src.ArtistName.Value));
    }
}