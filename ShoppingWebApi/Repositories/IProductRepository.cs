using ShoppingWebApi.Models;

namespace ShoppingWebApi.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}