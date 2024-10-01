using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingWebApi.EFCore;

[Table("order")]
public class Order
{
    [Key,Required]
    
    public int id { get; set; }
    public virtual Product Product { get; set; }
    public string name { get; set; } = String.Empty;
    public string address { get; set; } = String.Empty;
    public string phone { get; set; } = String.Empty;
}