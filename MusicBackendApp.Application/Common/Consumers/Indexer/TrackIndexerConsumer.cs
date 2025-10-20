using MassTransit;
using MusicBackendApp.Application.Common.Events.Created;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Common.Interfaces.Search;
using MusicBackendApp.Application.Common.SearchModels;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Common.Consumers.Indexer;

public class TrackIndexerConsumer : IConsumer<TrackCreatedEvent>
{
    private readonly ISearchService _searchService;
    private readonly IArtistRepository _artistRepository;

    public TrackIndexerConsumer(ISearchService searchService, IArtistRepository artistRepository)
    {
        _searchService = searchService;
        _artistRepository = artistRepository;
    }

    public async Task Consume(ConsumeContext<TrackCreatedEvent> context)
    {
        var trackEvent = context.Message; 
        
        string artistName = trackEvent.Title;  
        if (trackEvent.ArtistId != Guid.Empty)
        {
            var artistResult = await _artistRepository.GetByIdAsync(new ArtistId(trackEvent.ArtistId));
            if (artistResult.IsSuccess)
            {
                artistName = artistResult.Value.ArtistName.Value;
            }
        }

        // Мапуємо дані з події (та репозиторію) на TrackSearchModel
        var trackSearchModel = new TrackSearchModel
        {
            Id = trackEvent.TrackId.ToString(),
            Title = trackEvent.Title,
            ArtistName = artistName
        };

        // Викликаємо метод сервісу пошуку для індексації в Elasticsearch
        await _searchService.IndexTracksAsync(trackSearchModel);

        Console.WriteLine($"[Elasticsearch] Track '{trackSearchModel.Title}' (ID: {trackSearchModel.Id}) indexed.");
    }
}