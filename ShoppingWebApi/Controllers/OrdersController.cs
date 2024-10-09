using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebApi.Data;
using ShoppingWebApi.DTOs;
using ShoppingWebApi.Models;
using ShoppingWebApi.Repositories;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;  // AutoMapper instance
    private readonly ApplicationDbContext _context;

    public OrdersController(IOrderRepository orderRepository, IMapper mapper, ApplicationDbContext context)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders); // Map Order -> OrderDto
        return Ok(orderDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        var orderDto = _mapper.Map<OrderDto>(order); // Map Order -> OrderDto
        return Ok(orderDto);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = _mapper.Map<Order>(orderDto);
    
        var productExists = await _context.Products.AnyAsync(p => p.Id == orderDto.ProductId);
        if (!productExists)
        {
            return BadRequest(new { message = "Invalid ProductId" });
        }
        await _orderRepository.CreateAsync(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
    {
        if (id != orderDto.Id)
        {
            return BadRequest("Order ID mismatch.");
        }

        var productExists = await _context.Products.AnyAsync(p => p.Id == orderDto.ProductId);
        if (!productExists)
        {
            return BadRequest(new { message = "Invalid ProductId." });
        }

        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound("Order not found.");
        }

        _mapper.Map(orderDto, existingOrder);
        await _orderRepository.UpdateAsync(existingOrder);

        return NoContent(); 
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderRepository.DeleteAsync(id);
        return NoContent();
    }
}