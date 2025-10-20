using MusicBackendApp.Application.Common.SearchModels;

namespace MusicBackendApp.Application.Common.Interfaces.Search;

public interface ISearchService
{
    Task CreateIndexIfNotExistsAsync(string indexName); 
    Task IndexTracksAsync(TrackSearchModel model); 
    Task IndexArtistAsync(ArtistSearchModel model);
    Task<List<TrackSearchModel>> SearchTrackAsync(string query); 
    Task<List<ArtistSearchModel>> SearchArtistAsync(string query);
    Task ReindexAllDataAsync();
}