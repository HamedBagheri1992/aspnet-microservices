using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> Get([FromRoute] string userName)
        {
            var basket = await _basketRepository.GetBasketAsync(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> Post([FromBody] ShoppingCart basket)
        {
            return Ok(await _basketRepository.UpdateBasketAsync(basket));
        }


        [HttpDelete("{userName}")]
        public async Task<IActionResult> Delet([FromRoute] string userName)
        {
            await _basketRepository.DeleteBasketAsync(userName);
            return NoContent();
        }
    }
}
