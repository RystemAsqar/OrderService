using MassTransit;
using ShoppingWebApi.Contracts;
using ShoppingWebApi.Services.Interface;

namespace ShoppingWebApi.Services;

public class MessagePublisherService: IMessagePublisherService
{
    private readonly ILogger<MessagePublisherService> _logger;
    private readonly IPublishEndpoint _publishEndpoint;


    public MessagePublisherService(ILogger<MessagePublisherService> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task SentMessage(int orderId, string address)
    {
        _logger.LogInformation($"Sending order {orderId} to {address}");
        await _publishEndpoint.Publish(new MessageRecord(orderId, address));
    }
}