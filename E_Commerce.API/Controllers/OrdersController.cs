using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken ct)
        {
            return toActionResult(await orderService.CreateOrderAsync(orderDto, GetUserEmailFromToken(), ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken ct)
        {
            return toActionResult(await orderService.GetAllOrdersForUserAsync(GetUserEmailFromToken(), ct));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id, CancellationToken ct)
        {
            return toActionResult(await orderService.GetOrderByIdForUserAsync(id, GetUserEmailFromToken(), ct));
        }


        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken ct)
        {
            return toActionResult(await orderService.GetDeliveryMethodsAsync(ct));
        }
    }
}
