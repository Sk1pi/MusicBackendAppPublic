using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;

namespace MusicBackendApp.Application.Track.Queries.GetMusicById;

public class TrackIdVm : IMapWith<Domain.Entites.Track>
{
    public Guid TrackId { get; set; }
    public int Valume { get; set; }
    public string? Title { get; set; }
    public int DurationInSeconds  { get; set; }
    public string ArtistName { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entites.Track, TrackIdVm>()
            .ForMember(trackVm => trackVm.TrackId,
                opt => opt.MapFrom(track => track.Id.Value)) 
            .ForMember(trackVm => trackVm.Valume,
                opt => opt.MapFrom(track => track.Valume))
            .ForMember(trackVm => trackVm.Title,
                opt => opt.MapFrom(track => track.Title.Value)) 
            .ForMember(trackVm => trackVm.DurationInSeconds,
                opt => opt.MapFrom(track => track.Duration.TotalSeconds))
            .ForMember(trackVm => trackVm.ArtistName,
                opt => opt.MapFrom(track => track.Author.ArtistName.Value));

    }
}