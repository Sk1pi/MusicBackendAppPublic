using MassTransit;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Events.Login;

namespace MusicBackendApp.Application.Common.Consumers.Login;

public class UserLoginConsumer : IConsumer<UserLoginEvent>
{
    private readonly ILogger<UserLoginConsumer> _logger;

    public UserLoginConsumer(ILogger<UserLoginConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<UserLoginEvent> context)
    {
        _logger.LogInformation(
            $"[RabbitMQ] User logined! ID: {context.Message.UserId}, Name: {context.Message.UserName}");
        
        // Можна додати іншу логіку, наприклад, оновлення індексу пошуку, відправку email тощо.
        return Task.CompletedTask;
    }
}