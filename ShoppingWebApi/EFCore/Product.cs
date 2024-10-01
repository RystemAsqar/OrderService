using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebApi.EFCore;

[Table("product")]


public class Product
{
    [Key,Required]
    public int id { get; set; }
    public string name { get; set; } = String.Empty;
    public string brand { get; set; } = String.Empty;
    public int size { get; set; }
    public decimal price { get; set; }
}