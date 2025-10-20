using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events;

namespace MusicBackendApp.Application.Common.Consumers.AddRemove;

public class TrackAddedToFavoritesConsumer : IConsumer<TrackAddedToFavoritesEvent>
{
    private readonly ILogger<TrackAddedToFavoritesConsumer> _logger;

    public TrackAddedToFavoritesConsumer(ILogger<TrackAddedToFavoritesConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<TrackAddedToFavoritesEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] New track created! ID: {context.Message.TrackId}, User: {context.Message.UserId}");
        
        return Task.CompletedTask;
    }
}