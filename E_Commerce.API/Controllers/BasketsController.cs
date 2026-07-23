using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id, CancellationToken ct)
        {
            var Result = await _basketService.GetBasketAsync(id,ct:ct);
            return toActionResult(Result);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket, CancellationToken ct)
        {
            var Result = await _basketService.CreateOrUpdateBasketAsync(basket, ct:ct);
            return toActionResult(Result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var Result = await _basketService.DeleteBasketAsync(id, ct:ct);
            return toActionResult(Result);
        }

    }
}
