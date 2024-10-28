using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWebApi.Contracts;
using ShoppingWebApi.Data;
using ShoppingWebApi.DTOs;
using ShoppingWebApi.Models;
using ShoppingWebApi.Repositories;
using ShoppingWebApi.Services.Interface;

namespace ShoppingWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;  
    private readonly ApplicationDbContext _context;
    private readonly IMessagePublisherService _messagePublisher;
    private readonly IPublishEndpoint _publishEndpoint;
    

    public OrdersController(
        IOrderRepository orderRepository, 
        IMapper mapper, 
        ApplicationDbContext context, 
        IMessagePublisherService messagePublisher, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _context = context;
        _messagePublisher = messagePublisher;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllAsync();
        var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders); 
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
        var orderDto = _mapper.Map<OrderDto>(order); 
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
        
        var orderMessage = new OrderMessage
        {
            OrderId = orderDto.Id,
            CustomerName = orderDto.Name,
            OrderAddress = orderDto.Address
        };

        await _publishEndpoint.Publish(orderMessage);
        await _orderRepository.CreateAsync(order);
        await _messagePublisher.SentMessage(order.Id, order.Address);
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