using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events;

namespace MusicBackendApp.Application.Common.Consumers.AddRemove;

public class TrackRemovedFromFavoritesConsumer : IConsumer<TrackRemovedFromFavoritesEvent>
{
    private readonly ILogger<TrackRemovedFromFavoritesConsumer> _logger;

    public TrackRemovedFromFavoritesConsumer(ILogger<TrackRemovedFromFavoritesConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<TrackRemovedFromFavoritesEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] Track deleted! ID: {context.Message.TrackId}, UserId: {context.Message.UserId}");
        
        return Task.CompletedTask;
    }
}