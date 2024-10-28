using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ShoppingWebApi.DTOs;

public class OrderService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public OrderService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "rystemasqar",
            Password = "0055"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        // Declare a queue for delivery orders
        _channel.QueueDeclare(queue: "delivery_orders",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public async Task SendOrder(OrderDto orderMessage)
    {
        await Task.Run(() =>
        {
            // Serialize the order message to JSON
            var jsonMessage = JsonSerializer.Serialize(orderMessage);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            // Publish the message to the queue
            _channel.BasicPublish(exchange: "",
                routingKey: "delivery_orders",
                basicProperties: null,
                body: body);

            Console.WriteLine($"Order sent: {jsonMessage}");
        });
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}