using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories.Interfaces;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
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
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepository.UpdateBasketAsync(basket));
        }


        [HttpDelete("{userName}")]
        public async Task<IActionResult> Delet([FromRoute] string userName)
        {
            await _basketRepository.DeleteBasketAsync(userName);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            try
            {
                var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);
                if (basket is null)
                    return BadRequest();

                var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
                eventMessage.TotalPrice = basket.TotalPrice;
                await _publishEndpoint.Publish(eventMessage);

                await _basketRepository.DeleteBasketAsync(basket.UserName);
                return Accepted();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }
    }
}
