using ShoppingWebApi.Models;

namespace ShoppingWebApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(int id);
    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
}