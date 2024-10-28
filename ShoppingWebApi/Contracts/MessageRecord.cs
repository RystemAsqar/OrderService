namespace ShoppingWebApi.Contracts;

public record MessageRecord(int OrderId, String OrderAddress);


public class OrderMessage
{
    public int OrderId { get; set; }
    public String OrderAddress { get; set; }
    public String CustomerName { get; set; }
}