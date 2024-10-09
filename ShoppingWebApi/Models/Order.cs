namespace ShoppingWebApi.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    // Navigation property
    public Product Product { get; set; }
}