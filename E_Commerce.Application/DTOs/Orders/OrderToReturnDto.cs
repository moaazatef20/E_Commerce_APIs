using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.DTOs.Orders
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDTO ShippingAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public Decimal DeliveryCost { get; set; }
        public Decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
