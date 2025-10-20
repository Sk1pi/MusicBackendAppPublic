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
    private readonly IArtistRepository _artistRepository; // Можливо, потрібен для ArtistName

    public TrackIndexerConsumer(ISearchService searchService, IArtistRepository artistRepository) // Ін'єктуємо
    {
        _searchService = searchService;
        _artistRepository = artistRepository;
    }

    public async Task Consume(ConsumeContext<TrackCreatedEvent> context)
    {
        var trackEvent = context.Message; // Отримуємо дані з події

        // Часто події не містять всіх даних, необхідних для пошукової моделі.
        // Наприклад, ArtistName. Його потрібно отримати з БД через репозиторій.
        // Або модифікуй ArtistCreatedEvent, щоб він містив ArtistName.
        string artistName = trackEvent.Title;  // Заглушка
        if (trackEvent.ArtistId != Guid.Empty) // Перевірка, чи ArtistId є в події
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