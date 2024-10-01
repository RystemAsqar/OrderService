namespace ShoppingWebApi.Models;

public class OrderModel
{
    public int id { get; set; }
    public int product_id { get; set; }
    public string name { get; set; } = String.Empty;
    public string address { get; set; } = String.Empty;
    public string phone { get; set; } = String.Empty;
}