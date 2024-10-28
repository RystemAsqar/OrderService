using MassTransit;
using ShoppingWebApi.Contracts;

namespace DeliveryService.Api.Services;

public class MessageNotificationConsumer: IConsumer<MessageRecord>
{
    
    private readonly ILogger<MessageNotificationConsumer> _logger;

    public MessageNotificationConsumer(ILogger<MessageNotificationConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MessageRecord> context)
    {
        _logger.LogInformation($"Received message: {context.Message}");
        return Task.CompletedTask;
    }
}