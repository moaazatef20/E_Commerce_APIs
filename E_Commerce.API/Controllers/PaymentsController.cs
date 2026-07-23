using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [Authorize]
        [HttpPost("{basketsId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePaymentIntent(string basketsId, CancellationToken ct)
        {
            var result = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketsId, ct);
            return toActionResult(result);
        }

    }
}
