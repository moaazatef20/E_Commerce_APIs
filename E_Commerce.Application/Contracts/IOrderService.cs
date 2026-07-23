using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto,string userEmail, CancellationToken ct = default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForUserAsync(string userEmail, CancellationToken ct = default);
        Task<Result<OrderToReturnDto>> GetOrderByIdForUserAsync(Guid orderId, string userEmail, CancellationToken ct = default);
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(CancellationToken ct = default);
    }
}
