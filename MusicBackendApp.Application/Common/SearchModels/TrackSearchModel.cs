using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Common.SearchModels;

public class TrackSearchModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; } 

    public ArtistId ArtistId { get; set; }
}