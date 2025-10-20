using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;
using MusicBackendApp.Domain.Entites;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistById;

public class ArtistByIdVm : IMapWith<Artist>
{
    //public List<ArtistLookupDto> Artists { get; set; }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }    
    public int Subs { get; init; }

    public void Mapping(Profile profile) //профіль мапінгу AutoMapper), в якому ви реєструєте свої правила.
    {
        profile.CreateMap<Artist, ArtistByIdVm>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.ArtistName.Value))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Password,
                opt => opt.MapFrom(src => src.Password.Value))
            .ForMember(dest => dest.Subs,
                opt => opt.MapFrom(src => src.Subs));
    }
}

//IRequest використовується для команд/запитів, які відправляються через MediatR.