using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productRepository.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Product>> Get([FromRoute] string id)
        {
            var product = await _productRepository.GetProductAsync(id);
            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet("[action]/{categoryName}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategoryName([FromRoute] string categoryName)
        {
            var products = await _productRepository.GetProductByCategoryAsync(categoryName);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            await _productRepository.CreateProductAsync(product);
            return CreatedAtRoute(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            bool result = await _productRepository.UpdateProductAsync(product);
            if (result == false)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            bool result = await _productRepository.DeleteProductAsync(id);
            if (result == false)
                return BadRequest();

            return NoContent();
        }
    }
}
