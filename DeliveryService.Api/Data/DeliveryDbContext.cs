namespace DeliveryService.Api.Data;
using Microsoft.EntityFrameworkCore;

public class DeliveryDbContext : DbContext
{
    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) { }

    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
}

public class DeliveryOrder
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}
