using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/products
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // GET /api/products/{productId}
        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST /api/products (admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(productDto);
            if (product == null)
                return BadRequest("Product creation failed.");

            return Ok(product);
        }

        // PUT /api/products/{productId} (admin only)
        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productService.UpdateProductAsync(productId, productDto);
            return Ok("Product updated.");
        }
    }
} 