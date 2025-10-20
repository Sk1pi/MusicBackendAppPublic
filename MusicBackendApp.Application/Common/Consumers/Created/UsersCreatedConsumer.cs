using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events.Created;


namespace MusicBackendApp.Application.Common.Consumers.Created;

public class UsersCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UsersCreatedConsumer> _logger;

    public UsersCreatedConsumer(ILogger<UsersCreatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] New track created! ID: {context.Message.UserId}, Name: {context.Message.UserName}");
        
        return Task.CompletedTask;
    }
}