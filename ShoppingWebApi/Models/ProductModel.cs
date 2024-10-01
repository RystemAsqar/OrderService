namespace ShoppingWebApi.Models;

public class ProductModel
{
    public int id { get; set; }
    public string name { get; set; } = String.Empty;
    public string brand { get; set; } = String.Empty;
    public int size { get; set; }
    public decimal price { get; set; }
}