using MassTransit;
using MusicBackendApp.Application.Common.Events.Created;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Common.Interfaces.Search;
using MusicBackendApp.Application.Common.SearchModels;
using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Common.Consumers.Indexer;

public class ArtistIndexerConsumer : IConsumer<ArtistCreatedEvent>
{
    private readonly ISearchService _searchService;
    private readonly IArtistRepository _artistRepository;

    public ArtistIndexerConsumer(
        ISearchService searchService,
        IArtistRepository artistRepository)
    {
        _searchService = searchService;
        _artistRepository = artistRepository;
    }
    
    public async Task Consume(ConsumeContext<ArtistCreatedEvent> context)
    {
        var artistEvent = context.Message; // Отримуємо дані з події
        
        // Часто події не містять всіх даних, необхідних для пошукової моделі.
        // Наприклад, ArtistName. Його потрібно отримати з БД через репозиторій.
        // Або модифікуй ArtistCreatedEvent, щоб він містив ArtistName.
        string? artistName = artistEvent.ArtistName;
        if (artistEvent.ArtistId != Guid.Empty) // Перевірка, чи ArtistId є в події
        {
            var artistResult = await _artistRepository.GetByIdAsync(new ArtistId(artistEvent.ArtistId));
            if (artistResult.IsSuccess)
            {
                artistName = artistResult.Value.ArtistName.Value;
            }
        }

        var artistSerachModel = new ArtistSearchModel
        { 
            Id = artistEvent.ArtistId.ToString(),
            Name = artistName,
        };
        
        await _searchService.IndexArtistAsync(artistSerachModel);
        
        Console.WriteLine($"[Elasticsearch] Artist '{artistSerachModel.Name}' (ID: {artistSerachModel.Id}) indexed.");
    }
}
/*При отриманні події ArtistCreatedEvent або TrackCreatedEvent,
 перетворити її на відповідну пошукову модель (ArtistSearchModel або TrackSearchModel) 
 і відправити до Elasticsearch через ISearchService.*/