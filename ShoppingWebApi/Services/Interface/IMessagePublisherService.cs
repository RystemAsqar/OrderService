namespace ShoppingWebApi.Services.Interface;

public interface IMessagePublisherService
{
    Task SentMessage(int orderId, string orderName);
}