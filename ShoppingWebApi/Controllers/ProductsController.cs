using ShoppingWebApi.DTOs;
using ShoppingWebApi.Models;
using ShoppingWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper; // Inject AutoMapper

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products); // Map domain model to DTO
            return Ok(productDtos);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product); // Map domain model to DTO
            return Ok(productDto);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto); // Map DTO to domain model
            await _productRepository.CreateAsync(product);
            var createdProductDto = _mapper.Map<ProductDto>(product); // Map back to DTO for response
            return CreatedAtAction(nameof(GetProductById), new { id = createdProductDto.Id }, createdProductDto);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDto); // Map DTO to domain model
            await _productRepository.UpdateAsync(product);
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
