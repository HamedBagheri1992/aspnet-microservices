using Discount.API.Entities;
using Discount.API.Repositories.Intrefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{productName}")]
        public async Task<ActionResult<Coupon>> Get([FromRoute] string productName)
        {
            var coupon = await _discountRepository.GetDiscountAsync(productName);
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<ActionResult<Coupon>> Post([FromBody] Coupon coupon)
        {
            await _discountRepository.CreateDiscountAsync(coupon);
            return CreatedAtAction(nameof(Get), new { productName = coupon.Description }, coupon);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Coupon coupon)
        {
            bool result = await _discountRepository.UpdateDiscountAsync(coupon);
            if (result)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{productName}")]
        public async Task<IActionResult> Delete([FromRoute] string productName)
        {
            bool result = await _discountRepository.DeleteDiscountAsync(productName);
            if (result)
                return BadRequest();

            return NoContent();
        }
    }
}
