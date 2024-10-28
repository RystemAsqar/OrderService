
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryService.Api.Data;

namespace DeliveryService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryOrderController : ControllerBase
    {
        private readonly DeliveryDbContext _dbContext;

        public DeliveryOrderController(DeliveryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/DeliveryOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryOrder>>> GetAllOrders()
        {
            var orders = await _dbContext.DeliveryOrders.ToListAsync();
            return Ok(orders);
        }

        // GET: api/DeliveryOrder/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryOrder>> GetOrderById(int id)
        {
            var order = await _dbContext.DeliveryOrders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/DeliveryOrder
        [HttpPost]
        public async Task<ActionResult<DeliveryOrder>> CreateOrder(DeliveryOrder order)
        {
            _dbContext.DeliveryOrders.Add(order);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        // PUT: api/DeliveryOrder/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, DeliveryOrder order)
        {
            if (id != order.Id)
            {
                return BadRequest("Order ID mismatch.");
            }

            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/DeliveryOrder/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _dbContext.DeliveryOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _dbContext.DeliveryOrders.Remove(order);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _dbContext.DeliveryOrders.Any(e => e.Id == id);
        }
    }
}
