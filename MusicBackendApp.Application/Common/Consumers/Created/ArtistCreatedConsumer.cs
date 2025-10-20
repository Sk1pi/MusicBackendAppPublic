using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events.Created;

namespace MusicBackendApp.Application.Common.Consumers.Created;

public class ArtistCreatedConsumer : IConsumer<ArtistCreatedEvent>
{
    private readonly ILogger<ArtistCreatedConsumer> _logger;

    public ArtistCreatedConsumer(ILogger<ArtistCreatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<ArtistCreatedEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] New artist created! ID: {context.Message.ArtistId}, Name: {context.Message.ArtistName}");
        
        // Можна додати іншу логіку, наприклад, оновлення індексу пошуку, відправку email тощо.
        return Task.CompletedTask;
    }
}