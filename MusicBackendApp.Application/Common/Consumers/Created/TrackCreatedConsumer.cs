using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events.Created;

namespace MusicBackendApp.Application.Common.Consumers.Created;

public class TrackCreatedConsumer : IConsumer<TrackCreatedEvent>
{
    private readonly ILogger<TrackCreatedConsumer> _logger;

    public TrackCreatedConsumer(ILogger<TrackCreatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<TrackCreatedEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] New track created! ID: {context.Message.TrackId}, Title: {context.Message.Title}");
       
        return Task.CompletedTask;
    }
}