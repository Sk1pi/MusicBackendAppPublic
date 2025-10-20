using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Common.Interfaces.Search;
using MusicBackendApp.Application.Common.SearchModels;
// Додаткові using, які часто потрібні для Fluent API Elasticsearch
// ДОДАЙТЕ ЦІ USING ДЛЯ ВИРІШЕННЯ ПОМИЛОК
 // Для Field() в QueryString
using Elastic.Clients.Elasticsearch.Core.Bulk; // Для Document() в BulkAsync

namespace MusicBackendApp.Infrastructure.Configurations.Search.Services;

public class ElasticSearchService : ISearchService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticSettings _elasticSettings;
    private readonly IArtistRepository _artistRepository;
    private readonly ITrackRepository _trackRepository;

    public ElasticSearchService(
        IArtistRepository artistRepository,
        ITrackRepository trackRepository,
        IOptions<ElasticSettings> optionsMonitor)
    {
        _artistRepository = artistRepository;
        _trackRepository = trackRepository;
        
        _elasticSettings = optionsMonitor.Value;
        
        var settings = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
            .DefaultIndex(_elasticSettings.DefaultIndex);
        
        _client = new ElasticsearchClient(settings);
    }
    
    public async Task CreateIndexIfNotExistsAsync(string indexName)
    {
        if(!_client.Indices.Exists(indexName).Exists)
            await _client.Indices.CreateAsync(indexName);   
    }

    public async Task IndexTracksAsync(TrackSearchModel model)
    {
        var response = await _client.IndexAsync(model, idx =>
            idx.Index(_elasticSettings.DefaultIndex)
                .Id(model.Id)); 

        if (!response.IsValidResponse)
        {
            Console.WriteLine($"[ERROR] Failed to index track {model.Id}: {response.DebugInformation}");
            throw new Exception($"Failed to index track {model.Id}");
        }
    }

    public async Task IndexArtistAsync(ArtistSearchModel model)
    {
        var response = await _client.IndexAsync(model, idx =>
            idx.Index(_elasticSettings.DefaultIndex)
                .Id(model.Id)); 

        if (!response.IsValidResponse)
        {
            Console.WriteLine($"[ERROR] Failed to index artist {model.Id}: {response.DebugInformation}");
            throw new Exception($"Failed to index artist {model.Id}");
        }
    }

    public async Task<List<TrackSearchModel>> SearchTrackAsync(string query)
    {
        var response = await _client.SearchAsync<TrackSearchModel>(s => s
            .Index(_elasticSettings.DefaultIndex)
            .Query(q => q 
                .QueryString(qs => qs 
                    .Fields(new [] { "title", "artistName" }) 
                    .Query(query)))); 

        if (!response.IsValidResponse)
        {
            Console.WriteLine($"[ERROR] Search for tracks failed: {response.DebugInformation}");
            return new List<TrackSearchModel>(); 
        }

        return response.Documents.ToList();
    }

    public async Task<List<ArtistSearchModel>> SearchArtistAsync(string query)
    {
        var response = await _client.SearchAsync<ArtistSearchModel>(s => s
            .Index(_elasticSettings.DefaultIndex)
            .Query(q => q
                .QueryString(qs => qs
                    .Fields(new [] { "name" }) 
                    .Query(query))));

        if (!response.IsValidResponse)
        {
            Console.WriteLine($"[ERROR] Search for artists failed: {response.DebugInformation}");
            return new List<ArtistSearchModel>();
        }

        return response.Documents.ToList();
    }
    
    public async Task ReindexAllDataAsync()
    {
        Console.WriteLine("[INFO] Reindexing all data from PostgreSQL to Elasticsearch started.");
        
        var allArtists = await _artistRepository.GetAllAsync();
        var artistNamesLookup = allArtists.ToDictionary(
            artist => artist.Id, 
            artist => artist.ArtistName.Value); 
        
        var allTracks = await _trackRepository.GetAllAsync();
        var trackSearchModels = new List<TrackSearchModel>();
        
        foreach (var track in allTracks)
        {
            string artistNameForTrack = "Unknown Artist";
            if (track.ArtistId != null && artistNamesLookup.TryGetValue(track.ArtistId, out var foundArtistName))
            {
                artistNameForTrack = foundArtistName;
            }

            trackSearchModels.Add(new TrackSearchModel
            {
                Id = track.Id.Value.ToString(), 
                Title = track.Title.Value,
                ArtistName = artistNameForTrack
            });
        }
        
        if (trackSearchModels.Any())
        {
            var trackBulkResponse = await _client.BulkAsync(b => b
                .Index(_elasticSettings.DefaultIndex)
                .CreateMany(trackSearchModels.Select(model => new BulkCreateOperation<TrackSearchModel>(model) { Id = model.Id })));

            if (!trackBulkResponse.IsValidResponse)
            {
                Console.WriteLine($"[ERROR] Bulk indexing tracks failed: {trackBulkResponse.DebugInformation}");
            }
            else
            {
                Console.WriteLine($"[INFO] Successfully indexed {trackSearchModels.Count} tracks.");
            }
        }
        
        var artistSearchModels = allArtists.Select(artist => new ArtistSearchModel
        {
            Id = artist.Id.Value.ToString(),
            Name = artist.ArtistName.Value
        }).ToList();
        
        if (artistSearchModels.Any())
        {
            var artistBulkResponse = await _client.BulkAsync(b => b
                .Index(_elasticSettings.DefaultIndex)
                .CreateMany(artistSearchModels.Select(model => new BulkCreateOperation<ArtistSearchModel>(model) { Id = model.Id })));

            if (!artistBulkResponse.IsValidResponse)
            {
                Console.WriteLine($"[ERROR] Bulk indexing artists failed: {artistBulkResponse.DebugInformation}");
            }
            else
            {
                Console.WriteLine($"[INFO] Successfully indexed {artistSearchModels.Count} artists.");
            }
        }

        Console.WriteLine("[INFO] Reindexing all data from PostgreSQL to Elasticsearch completed.");
    }
}