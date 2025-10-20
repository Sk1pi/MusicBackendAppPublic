using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;

namespace MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

public class TrackLookupDto : IMapWith<Domain.Entites.Track>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string FilePath { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entites.Track, TrackLookupDto>()
            .ForMember(trackDto => trackDto.Id,
                opt => opt.MapFrom(src => src.Id.Value)) 
            .ForMember(trackDto => trackDto.Title,
                opt => opt.MapFrom(src => src.Title.Value)) 
            .ForMember(trackDto => trackDto.ArtistName,
                opt => opt.MapFrom(src => src.Author.ArtistName.Value)) 
            .ForMember(dto => dto.FilePath,
                opt => opt.MapFrom(src => src.FilePath));
    }
}