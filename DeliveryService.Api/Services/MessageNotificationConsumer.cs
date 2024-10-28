using DeliveryService.Api.Data;
using MassTransit;
using ShoppingWebApi.Contracts;


namespace DeliveryService.Api.Services
{
    public class MessageNotificationConsumer : IConsumer<OrderMessage>
    {
        private readonly ILogger<MessageNotificationConsumer> _logger;
        private readonly DeliveryDbContext _dbContext;

        public MessageNotificationConsumer(ILogger<MessageNotificationConsumer> logger, DeliveryDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<OrderMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation($"Received order message: {message.OrderId}, {message.CustomerName}, {message.OrderAddress}");

            var deliveryOrder = new DeliveryOrder
            {
                OrderId = message.OrderId,
                Name = message.CustomerName,
                Address = message.OrderAddress
            };

            await _dbContext.DeliveryOrders.AddAsync(deliveryOrder);
            await _dbContext.SaveChangesAsync();
        }
    }
}
