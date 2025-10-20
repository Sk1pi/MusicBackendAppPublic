using MusicBackendApp.Application.Common.SearchModels;

namespace MusicBackendApp.Application.Common.Interfaces.Search;

public interface ISearchService
{
    Task CreateIndexIfNotExistsAsync(string indexName); // Для створення індексу
    
    Task IndexTracksAsync(TrackSearchModel model); // Для індексування треку
    Task IndexArtistAsync(ArtistSearchModel model); // Для індексування артиста
    
    Task<List<TrackSearchModel>> SearchTrackAsync(string query); // Для пошуку треків
    Task<List<ArtistSearchModel>> SearchArtistAsync(string query); // Для пошуку артистів
    
    //Task DeleteTrackFromIndexAsync(string trackId); // Для видалення треку з індексу
    //Task DeleteArtistFromIndexAsync(string artistId); // Для видалення артиста з індексу
    
    Task ReindexAllDataAsync();
}