using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebApi.EFCore;
using ShoppingWebApi.Models;

namespace ShoppingWebApi.Controllers
{
    
    [ApiController]
    public class ShoppingApiController : ControllerBase
    {

        private readonly EF_DataContext _context;

        public ShoppingApiController(EF_DataContext context)
        {
            _context = context;
        }

        // GET: api/<ShoppingApiController>
        [HttpGet]
        [Route("api/[controller]/GetProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }


        // GET api/<ShoppingApiController>/5
        [HttpGet]
        [Route("api/[controller]/GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        // POST api/<ShoppingApiController>
        [HttpPost]
        [Route("api/[controller]/SaveOrder")]
        public async Task<IActionResult> PostAsync([FromBody] OrderModel orderModel)
        {
            try
            {
                ResponseType type = ResponseType.Success;
                Order dbTable = new Order();
                    // POST - INSERT
                    dbTable.phone = orderModel.phone;
                    dbTable.address = orderModel.address;
                    dbTable.name = orderModel.name;
                    dbTable.Product = await _context.Products.Where(f => f.id == orderModel.product_id).FirstOrDefaultAsync();
                    await _context.Orders.AddAsync(dbTable);

                await _context.SaveChangesAsync();
                return Ok(ResponseHandler.GetAppResponse(type, orderModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        // PUT api/<ShoppingApiController>/5
        [HttpPut]
        [Route("api/[controller]/UpdateOrder")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] OrderModel orderModel)
        {
            Order dbTable = await _context.Orders.Where(d => d.id.Equals(orderModel.id)).FirstOrDefaultAsync();
            try
            {
                if (orderModel.id > 0)
                {
                    //PUT-UPDATE
                    dbTable = _context.Orders.Where(d => d.id.Equals(orderModel.id)).FirstOrDefault();
                    if (dbTable != null)
                    {
                        dbTable.phone = orderModel.phone;
                        dbTable.address = orderModel.address;
                    }
                }
                return Ok(ResponseHandler.GetAppResponse(ResponseType.Success, orderModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex));
            }
        }


        // DELETE api/<ShoppingApiController>/5
        [HttpDelete]
        [Route("api/[controller]/DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                ResponseType type = ResponseType.Success;

                // Find the order by id asynchronously
                var order = await _context.Orders
                    .Where(d => d.id == id)
                    .FirstOrDefaultAsync();

                if (order != null)
                {
                    // Remove the order from the database
                    _context.Orders.Remove(order);

                    // Save the changes asynchronously
                    await _context.SaveChangesAsync();

                    return Ok(ResponseHandler.GetAppResponse(type, "Deleted Successfully!"));
                }
                else
                {
                    return NotFound("Order not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHandler.GetExceptionResponse(ex)); 
            }
        }

    }
}
